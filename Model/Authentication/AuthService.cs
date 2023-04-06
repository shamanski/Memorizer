using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.DTO;
using Model.Entities;


    public class AuthService : IAuthService
{        
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;


    public AuthService(IConfiguration config, UserManager<ApplicationUser> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    public async Task<AuthResult> RegisterAsync(RegisterDTO registerDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", errors)}");
        }

        await _userManager.AddToRoleAsync(user, "User");

        var jwtToken = GenerateJwtToken(user);
        return new AuthResult
        {
            
        };
    }

    public async Task<AuthResult> LoginAsync(LoginDTO loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
        {
            throw new InvalidOperationException("Invalid email or password");
        }

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
        {
            throw new InvalidOperationException("Invalid email or password");
        }

        var jwtToken = GenerateJwtToken(user);
        return await GenerateJwtToken(user);
    }

    private async Task<AuthResult> GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new AuthResult
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}
    

