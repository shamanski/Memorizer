using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands
{
    public abstract class BotCommand
    {
        public abstract string Name { get; }
        protected readonly ChatController chat;

        protected BotCommand(ChatController chatController) => chat = chatController;

        public abstract Task<bool> Execute(ReversoConsole.DbModel.User user, Message message);

        public abstract Task<bool> Next(ReversoConsole.DbModel.User user, Message message);
    }
}
