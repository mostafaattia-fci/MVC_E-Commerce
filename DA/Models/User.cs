using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? Role { get; set; } = "Customer";

        public ICollection<Address>? Addresses { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
