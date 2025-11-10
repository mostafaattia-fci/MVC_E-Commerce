using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty; // Stripe / PayPal / Fawry

        public string? TransactionId { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public bool IsSuccessful { get; set; }
    }
}
