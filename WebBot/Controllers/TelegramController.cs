using Memorizer.DbModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using Model.Services;

namespace WebBot.Controllers
{
    public class TelegramController : Controller
    {
        private readonly WebAppContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TelegramController(WebAppContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetCode()
        {
            var code = GenerateUniqueCode();
            ViewData["Code"] = code;

            var userId = _userManager.GetUserId(User);
            var telegramCode = new TelegramCode { UserId = userId, Code = code };
            _dbContext.TelegramCodes.Add(telegramCode);
            _dbContext.SaveChanges();

            return View();
        }

        private string GenerateUniqueCode()
        {
            var random = new Random();
            var code = string.Empty;

            // Генерируем уникальный код из 6 цифр
            do
            {
                code = random.Next(100000, 999999).ToString();
            }
            while (_dbContext.TelegramCodes.Any(c => c.Code == code));

            return code;
        }
    }
}
