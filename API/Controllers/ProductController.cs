using API.Response;
using BLL.Services.Category;
using BLL.Services.Product;
using BLL.Services.Review_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IReviewService reviewService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
        }

   
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? categoryId)
        {
            try
            {
                var products = string.IsNullOrEmpty(categoryId)
                    ? await _productService.GetAllAsync()
                    : await _productService.GetByCategoryAsync(categoryId);

                return Ok(ResponseHelper.Success(products));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<string>(ex.Message));
            }
        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(ResponseHelper.Fail<string>("Product ID is required"));

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound(ResponseHelper.Fail<string>("Product not found"));

            var reviews = await _reviewService.GetReviewsForProductAsync(id);

            var result = new
            {
                Product = product,
                Reviews = reviews
            };

            return Ok(ResponseHelper.Success(result));
        }
    }
}
