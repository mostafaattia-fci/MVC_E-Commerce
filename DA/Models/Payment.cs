using DAL.Enums;
using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class Payment : BaseModel
    {
        public string OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; } // Stripe / PayPal / Fawry

        public string? TransactionId { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public bool IsSuccessful { get; set; }
    }
}
