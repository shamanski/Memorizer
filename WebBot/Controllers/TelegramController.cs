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

            // Сохраняем код в базе данных вместе с идентификатором пользователя
            var userId = _userManager.GetUserId(User);
            var telegramCode = new TelegramCode { UserId = userId, Code = code };
            _dbContext.TelegramCodes.Add(telegramCode);
            _dbContext.SaveChanges();

            return View();
        }

        [HttpPost]
        public IActionResult VerifyCode(string code)
        {
            var userId = _userManager.GetUserId(User);
            var telegramCode = _dbContext.TelegramCodes.FirstOrDefault(c => c.UserId == userId && c.Code == code);

            if (telegramCode == null)
            {
                ModelState.AddModelError(string.Empty, "Неправильный код.");
                return View("GetCode");
            }

            // Удаляем код из базы данных, так как он больше не нужен
            _dbContext.TelegramCodes.Remove(telegramCode);
            _dbContext.SaveChanges();

            // Обновляем информацию в профиле пользователя, что он привязал свой аккаунт к телеграму
            var user = _userManager.GetUserAsync(User).Result;
            user.IsTelegramVerified = true;
            _userManager.UpdateAsync(user).Wait();

            TempData["SuccessMessage"] = "Аккаунт телеграмма успешно привязан!";
            return RedirectToAction("Index", "Home");
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
