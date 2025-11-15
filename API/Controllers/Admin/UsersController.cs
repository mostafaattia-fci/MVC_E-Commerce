using API.Response;
using BLL.DTOs.Admin;
using BLL.Services.AdminUserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IAdminUserService _userService;

        public UsersController(IAdminUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersWithRolesAsync();
            return Ok(ResponseHelper.Success(users));
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var viewModel = await _userService.GetUserRolesAsync(id);
            if (viewModel == null)
            {
                return NotFound(ResponseHelper.Fail<UserRolesViewModel>("User not found"));
            }
            return Ok(ResponseHelper.Success(viewModel));
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> UpdateUserRoles(string id, [FromBody] UserRolesViewModel viewModel)
        {
            if (id != viewModel.UserId)
            {
                return BadRequest(ResponseHelper.Fail<object>("User ID mismatch"));
            }

            try
            {
                await _userService.UpdateUserRolesAsync(viewModel);
                return Ok(ResponseHelper.Success("Roles updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseHelper.Fail<object>("User not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>(ex.Message));
            }
        }
    }
}