using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Model.DTO;

public interface IIdentityUserService
{
    Task<IdentityResult> CreateUserAsync(RegisterDTO registerDto);
    Task<IdentityResult> AddToRoleAsync(string email, string roleName);
    Task<bool> CheckPasswordAsync(string email, string password);
    Task<IEnumerable<string>> GetRolesAsync(string email);
}
