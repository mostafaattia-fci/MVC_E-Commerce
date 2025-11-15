using BLL.DTOs.Admin;
using DA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.AdminUserService
{
    public class AdminUserService : IAdminUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserAdminDto>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserAdminDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserAdminDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? "N/A",
                    Roles = string.Join(", ", roles)
                });
            }
            return userDtos;
        }

        public async Task<UserRolesViewModel?> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? "N/A",
                Roles = allRoles.Select(role => new RoleDto
                {
                    RoleName = role.Name ?? "N/A",
                    IsSelected = userRoles.Contains(role.Name ?? "")
                }).ToList()
            };
            return viewModel;
        }

        public async Task UpdateUserRolesAsync(UserRolesViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.UserId);
            if (user == null) throw new KeyNotFoundException("User not found");

            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = viewModel.Roles.Where(r => r.IsSelected).Select(r => r.RoleName);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        }
    }
}
