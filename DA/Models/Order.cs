using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending / Shipped / Delivered / Cancelled

        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

        public Payment? Payment { get; set; }
    }
}
