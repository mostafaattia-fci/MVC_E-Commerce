using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class Review : BaseModel
    {
        public string ProductId { get; set; }
        public Product? Product { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
