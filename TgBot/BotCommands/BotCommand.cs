using System.Threading.Tasks;
using Telegram.Bot.Types;
using ReversoConsole.DbModel;

namespace TgBot.BotCommands
{
    public abstract class BotCommand
    {
        public abstract string Name { get; }
        protected readonly ChatController chat;

        protected BotCommand(ChatController chatController) => chat = chatController;

        public abstract Task<bool> Execute(IUser user, Message message, params string[] param);

        public abstract Task<bool> Next(IUser user, Message message);
    }
}
