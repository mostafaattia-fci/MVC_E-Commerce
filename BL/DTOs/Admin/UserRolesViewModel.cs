namespace BLL.DTOs.Admin
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<RoleDto> Roles { get; set; } = new();
    }
}
