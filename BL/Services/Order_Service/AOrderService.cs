using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs.OrderDTOs;
using DAL.Enums;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace BLL.Services.Order_Service
{
    public class AOrderService : IAOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AOrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(string userId)
        {
            var orders = await _unitOfWork.Orders
                .GetQueryable()
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderHistoryDto>>(orders);
        }

        public async Task<OrderDTO> GetOrderDetailsAsync(string orderId)
        {
            var order = await _unitOfWork.Orders
                .GetQueryable()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);

            return _mapper.Map<OrderDTO>(order);
        }
        public async Task CancelOrderAsync(string orderId, string userId)
        {
            var order = await _unitOfWork.Orders
                .GetQueryableWithTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                throw new Exception("Order not found or you do not have permission to cancel it.");

            order.Status = OrderStatus.Cancelled;
            order.ModifiedOnUtc = DateTime.UtcNow;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();
        }
    }
}






