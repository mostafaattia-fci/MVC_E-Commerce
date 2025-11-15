using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTOs.Admin;
using DAL.Enums;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.AdminOrderService
{
    public class AdminOrderService : IAdminOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminOrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderAdminListDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Orders.GetQueryable()
                .ProjectTo<OrderAdminListDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<OrderAdminDetailsDto?> GetOrderDetailsAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var order = await _unitOfWork.Orders.GetQueryable()
                .Where(o => o.Id == orderId)
                .ProjectTo<OrderAdminDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return order;
        }

        public async Task UpdateOrderStatusAsync(string orderId, OrderStatus newStatus, CancellationToken cancellationToken = default)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId, cancellationToken);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            order.Status = newStatus;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
