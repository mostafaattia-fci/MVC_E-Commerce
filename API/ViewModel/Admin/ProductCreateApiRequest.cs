using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.ViewModel.Admin
{
    public class ProductCreateApiRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public string CategoryId { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}