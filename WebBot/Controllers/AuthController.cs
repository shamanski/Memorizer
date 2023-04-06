using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Entities;

namespace WebBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IIdentityUserService _userManager;

        public AuthController(IAuthService authService, IIdentityUserService userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResult = await _authService.LoginAsync(model);

            if (!authResult.Success)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            return Ok(authResult);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateUserAsync(model);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Errors.FirstOrDefault()?.Description });
            }

            var authResult = await _authService.LoginAsync(new LoginDTO
            {               
                Email = model.Email,
                Password = model.Password
            });

            return Ok(authResult);
        }
    }
}
