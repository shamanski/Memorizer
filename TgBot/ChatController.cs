using Model.Services;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBot
{
    public class ChatController
    {

        private delegate bool nextStep(Message message);
        private readonly TelegramBotClient bot;
        private readonly IConfiguration Configuration;
        private WebAppContext _context;
        public WebAppContext GetContext {get => _context;}

        public ChatController(TelegramBot tgBot, IConfiguration configuration, WebAppContext context)
        {
            bot = tgBot.GetBot().Result;
            Configuration = configuration;
            _context = context;
        }
       
        public async Task ReplyMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: messageText,
                replyMarkup: message.ReplyMarkup,
                cancellationToken: new CancellationToken());
        }

        public async Task EditMessageAsync(Message message)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            _ = await bot.EditMessageReplyMarkupAsync(
                chatId: chatId,
                messageId: messageId,
                replyMarkup: message.ReplyMarkup);
            await bot.AnswerCallbackQueryAsync(message.Caption);
        }

        public async Task CallbackAsync(Message message)
        {
            await bot.AnswerCallbackQueryAsync(message.Caption);
        }

        public string GetFile(Message message)
        {
            
            return @"https://api.telegram.org/file/bot"+Configuration["Bot:Token"]+"/"+bot.GetFileAsync(message.Document.FileId).Result.FilePath;
        }


    }
}
