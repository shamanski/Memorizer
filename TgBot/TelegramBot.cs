using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgBot
{
    public class TelegramBot
    {
        private readonly IConfiguration _configuration;
        private TelegramBotClient _botClient;
        
        public async Task<TelegramBotClient>  GetBot()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _botClient = new TelegramBotClient("5065451258:AAF73Y8um3nU30RXEPQ59kVq98XnUTYYfeg");
            var hook = $"https://e8a7-176-118-153-198.ngrok.io/api/message";
            await _botClient.SetWebhookAsync(hook);
            return _botClient;
        }
    }
}
