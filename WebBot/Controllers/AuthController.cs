using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Entities;
using Model.Services;

namespace WebBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IIdentityUserService _userManager;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IIdentityUserService userManager, IUserService userService)
        {
            _authService = authService;
            _userManager = userManager;
            _userService = userService;
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
            await _userService.AddUserAsync(new Memorizer.DbModel.User { Email = model.Email, Name = model.Name });
            return Ok(authResult);
        }
    }
}
