using BLL.DTOs.Admin;
using DAL.Enums;

namespace BLL.Services.AdminOrderService
{
    public interface IAdminOrderService
    {
        Task<IEnumerable<OrderAdminListDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
        Task<OrderAdminDetailsDto?> GetOrderDetailsAsync(string orderId, CancellationToken cancellationToken = default);
        Task UpdateOrderStatusAsync(string orderId, OrderStatus newStatus, CancellationToken cancellationToken = default);
    }
}
