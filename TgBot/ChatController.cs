using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReversoConsole.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBot.BotCommands;
using TgBot.Keybords;

namespace TgBot
{
    public class ChatController
    {
        
        private delegate bool nextStep(Message message);
        private readonly TelegramBotClient bot;
        private readonly IConfiguration Configuration;

        public ChatController(TelegramBot tgBot, IConfiguration configuration)
        {
            bot = tgBot.GetBot().Result;
            Configuration = configuration;
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
