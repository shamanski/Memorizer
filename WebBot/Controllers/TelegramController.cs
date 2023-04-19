using Memorizer.DbModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using Model.Services;

namespace WebBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelegramController : Controller
    {
        private readonly WebAppContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public TelegramController(WebAppContext dbContext, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet("Code")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<string>> GetCode()
        {
            var code = GenerateUniqueCode();

            var email = _userManager.GetUserId(User);
            var appUser = _userService.GetUserByEmailAsync(email);
            var telegramCode = new TelegramCode { UserId = appUser.Result.Email, Code = code };
            _dbContext.TelegramCodes.Add(telegramCode);
            await _dbContext.SaveChangesAsync();

            return code;
        }

        private string GenerateUniqueCode()
        {
            var random = new Random();
            var code = string.Empty;

            do
            {
                code = random.Next(100000, 999999).ToString();
            }
            while (_dbContext.TelegramCodes.Any(c => c.Code == code));

            return code;
        }
    }
}
