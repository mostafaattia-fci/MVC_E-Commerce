using API.Response;
using BLL.DTOs.Admin;
using BLL.Services;
using ECommerce.API.ViewModel.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IAdminProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IAdminProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProductsAsync(cancellationToken);
            return Ok(ResponseHelper.Success(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
            if (product == null)
                return NotFound(ResponseHelper.Fail<ProductAdminDto>("Product not found"));

            return Ok(ResponseHelper.Success(product));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateApiRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string? imageUrl = null;
                if (request.ImageFile != null)
                {
                    imageUrl = await SaveImageAsync(request.ImageFile, cancellationToken);
                }

                var productDto = new CreateProductDto
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock,
                    CategoryId = request.CategoryId,
                    ImageURL = imageUrl
                };

                var newProduct = await _productService.CreateProductAsync(productDto, cancellationToken);
                return Ok(ResponseHelper.Success(newProduct, "Product created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<ProductAdminDto>(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] ProductEditApiRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                return BadRequest(ResponseHelper.Fail<object>("ID mismatch"));
            }

            try
            {
                string? imageUrl = request.ExistingImageUrl;
                if (request.ImageFile != null)
                {
                    imageUrl = await SaveImageAsync(request.ImageFile, cancellationToken);
                    DeleteImage(request.ExistingImageUrl);
                }

                var updateDto = new UpdateProductDto
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock,
                    CategoryId = request.CategoryId,
                    ImageUrl = imageUrl
                };

                await _productService.UpdateProductAsync(updateDto, cancellationToken);
                return Ok(ResponseHelper.Success("Product updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseHelper.Fail<object>("Product not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                await _productService.DeleteProductAsync(id, cancellationToken);
                return Ok(ResponseHelper.Success("Product deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseHelper.Fail<object>("Product not found"));
            }
        }

        private async Task<string?> SaveImageAsync(IFormFile imageFile, CancellationToken cancellationToken)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(wwwRootPath, "images", "products");

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(imagePath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream, cancellationToken);
            }

            return "/images/products/" + fileName;
        }

        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            try
            {
                var relativePath = imageUrl.TrimStart('/');
                relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
                var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}