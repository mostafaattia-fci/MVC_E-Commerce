using DAL.Enums;

namespace BLL.DTOs.Admin
{
    public class OrderAdminDetailsDto
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderItemAdminDto> Items { get; set; } = new();
    }
}
