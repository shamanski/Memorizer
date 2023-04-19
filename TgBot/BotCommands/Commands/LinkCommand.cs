using Memorizer.DbModel;
using Model.Services;
using ReversoApi.Models;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using User = Memorizer.DbModel.User;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/link - Привязать к веб-аккаунту")]
    public class LinkCommand : BotCommand, IBotCommand 
    {
        public override string Name { get; } = "/link";
        private IUserService _userService;

        public LinkCommand(ChatController chatController, IUserService userService) : base(chatController)
        {
            this._userService = userService;
        }

        public async override Task<bool> Execute(User user, Message message, params string[] args)
        {           
            message.Text = $"Введите код с сайта:";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);       
            return true;
        }

        public async override Task<bool> Next(User user, Message message)
        {
            try
            {
                await _userService.LinkTelegramAccountAsync(user, message.Text, user.TelegramId);
            }
            catch (InvalidOperationException ex)
            {
                message.Text = ex.Message;
            }
            await chat.ReplyMessage(message);
            return false;
        }
    }
}
