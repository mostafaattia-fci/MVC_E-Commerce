using BLL.DTOs.Admin;

namespace BLL.Services.AdminUserService
{
    public interface IAdminUserService
    {
        Task<IEnumerable<UserAdminDto>> GetAllUsersWithRolesAsync();
        Task<UserRolesViewModel?> GetUserRolesAsync(string userId);
        Task UpdateUserRolesAsync(UserRolesViewModel viewModel);
    }
}
