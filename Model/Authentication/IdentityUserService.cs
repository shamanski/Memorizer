using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Model.Entities;
using Model.Services;

public class IdentityUserService : IIdentityUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityUserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<IdentityResult> AddToRoleAsync(string email, string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CheckPasswordAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> CreateUserAsync(RegisterDTO registerDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        return result;
    }

    public async Task<IEnumerable<string>> GetRolesAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new ArgumentException($"User with email '{email}' not found");
        }

        var roles = await _userManager.GetRolesAsync(user);

        return roles;
    }

}
