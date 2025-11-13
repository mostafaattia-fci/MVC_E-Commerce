using BLL.DTOs.Admin;
using BLL.Services.AdminUserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IAdminUserService _userService;

        public UsersController(IAdminUserService userService)
        {
            _userService = userService;
        }

        // GET: /Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersWithRolesAsync();
            return View(users);
        }

        // GET: /Admin/Users/ManageRoles/{id}
        public async Task<IActionResult> ManageRoles(string id)
        {
            var viewModel = await _userService.GetUserRolesAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: /Admin/Users/ManageRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel viewModel)
        {
            try
            {
                await _userService.UpdateUserRolesAsync(viewModel);
                TempData["SuccessMessage"] = "Roles updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Failed to update roles: {ex.Message}");
                // (يجب إعادة ملء النموذج إذا فشل)
                var freshViewModel = await _userService.GetUserRolesAsync(viewModel.UserId);
                return View(freshViewModel);
            }
        }
    }
}