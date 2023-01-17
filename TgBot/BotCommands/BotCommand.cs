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

        public abstract Task<bool> Execute(ReversoConsole.DbModel.User user, Message message, params string[] param);

        public abstract Task<bool> Next(User user, Message message);
    }
}
