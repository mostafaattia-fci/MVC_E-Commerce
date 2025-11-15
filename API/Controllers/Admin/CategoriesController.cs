using API.Response;
using BLL.DTOs.Admin;
using BLL.Services.AdminCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly IAdminCategoryService _categoryService;

        public CategoriesController(IAdminCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            return Ok(ResponseHelper.Success(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
            if (category == null)
            {
                return NotFound(ResponseHelper.Fail<CategoryAdminDto>("Category not found"));
            }
            return Ok(ResponseHelper.Success(category));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoryCreateDto categoryDto, CancellationToken cancellationToken)
        {
            try
            {
                var newCategory = await _categoryService.CreateCategoryAsync(categoryDto, cancellationToken);
                return Ok(ResponseHelper.Success(newCategory, "Category created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseHelper.Fail<CategoryAdminDto>(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] CategoryAdminDto categoryDto, CancellationToken cancellationToken)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest(ResponseHelper.Fail<CategoryAdminDto>("ID mismatch"));
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(categoryDto, cancellationToken);
                return Ok(ResponseHelper.Success("Category updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseHelper.Fail<CategoryAdminDto>("Category not found"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseHelper.Fail<CategoryAdminDto>(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id, cancellationToken);
                return Ok(ResponseHelper.Success("Category deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseHelper.Fail<object>("Category not found"));
            }
        }
    }
}