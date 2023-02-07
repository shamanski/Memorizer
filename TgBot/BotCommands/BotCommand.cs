using System.Threading.Tasks;
using Telegram.Bot.Types;
using Model.Services;
using User = Memorizer.DbModel.User;

namespace TgBot.BotCommands
{
    public abstract class BotCommand
    {
        public abstract string Name { get; }
        protected readonly ChatController chat;

        protected BotCommand(ChatController chatController) => chat = chatController;

        public abstract Task<bool> Execute(User user, WebAppContext context, Message message, params string[] param);

        public abstract Task<bool> Next(User user, WebAppContext context, Message message);
    }
}
