using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Review>? Reviews { get; set; }
    }
}
