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
    static class ChatController
    {
        
        private delegate bool nextStep(Message message);
        static private TelegramBotClient bot;
        public static CommandService Command { get; set; }
        public static UserController Users { get; set; }

       
        static ChatController()
        {
            Users = new UserController();
            Command = new CommandService();
        }
        
        public static void SetBot(string token)
        {
            bot = new TelegramBotClient(token);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            using var cts = new CancellationTokenSource();
            bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token);
        }

        public static Message OnMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            var user = Users.GetUser(message.From.Id.ToString());
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            Command.Execute(user, message);
            return null;

        }

        public static async void ReplyMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            Message sentMessage = await bot.SendTextMessageAsync(
                chatId: chatId,
                text: messageText,
                replyMarkup: message.ReplyMarkup,
                cancellationToken: new CancellationToken());
        }

        public static async void EditMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            Message sentMessage = await bot.EditMessageReplyMarkupAsync(
                chatId: chatId,
                messageId: messageId,
                replyMarkup: message.ReplyMarkup);
            Message callBack = new Message();
            await bot.AnswerCallbackQueryAsync(message.Caption);
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                if (update.Type == UpdateType.CallbackQuery) { OnCallBack(update.CallbackQuery); return; }

                if (update.Message?.Type != MessageType.Text)
                    return;
                if (update.Message?.Type == MessageType.Text) OnMessage(update.Message);
                return;
            });
        }

        private async static void OnCallBack(CallbackQuery data) 
        {
            var user = Users.GetUser(data.From.Id.ToString());
            data.Message.Text = data.Data;
            data.Message.Caption = data.Id;
            await Task.Run(() => Command.Execute(user, data.Message));
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
