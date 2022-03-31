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
        private readonly IConfiguration Configuration;
        private TelegramBotClient _botClient;
        
        public TelegramBot(IConfiguration configuration)
        {
            Configuration = configuration;  
        }

        public async Task<TelegramBotClient>  GetBot()
        {
            if (_botClient != null)
            {
                return _botClient;
            }
            
            _botClient = new TelegramBotClient(Configuration["Bot:Token"]);
            await _botClient.SetWebhookAsync(Configuration["Bot:Hook"]);
            return _botClient;
        }
    }
}
