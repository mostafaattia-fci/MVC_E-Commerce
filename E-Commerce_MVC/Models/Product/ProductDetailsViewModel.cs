using BLL.DTOs.Product;
using BLL.DTOs.ReviewsDTOs;

namespace E_Commerce_MVC.Models.Product
{
    public class ProductDetailsViewModel
    {
        public ProductDTO Product { get; set; } = new ProductDTO();
        public int Quantity { get; set; } = 1;
        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}
