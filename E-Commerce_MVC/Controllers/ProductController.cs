using BLL.DTOs.ReviewsDTOs;
using BLL.Services.Category;
using BLL.Services.Product;
using BLL.Services.Review_Service;
using E_Commerce_MVC.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;

        public ProductController(IProductService productService, ICategoryService categoryService, IReviewService reviewService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
        }
        public async Task<IActionResult> Index(string? categoryId)
        {
            var categories = await _categoryService.GetAllAsync();

            var products = !string.IsNullOrEmpty(categoryId)
                ? await _productService.GetByCategoryAsync(categoryId)
                : await _productService.GetAllAsync();

            var viewModel = new ProductListViewModel
            {
                Categories = categories,
                Products = products,
                SelectedCategoryId = categoryId
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Product ID is required.");

            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var reviews = await _reviewService.GetReviewsForProductAsync(id);
            

                var viewModel = new ProductDetailsViewModel
                {
                    Product = product,
                    Reviews = reviews,
                    Quantity = 1
                };

            return View(viewModel);
        }
    }
}
