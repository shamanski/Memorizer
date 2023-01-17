using System.Threading.Tasks;
using Telegram.Bot.Types;
using Memorizer.DbModel;
using User = Memorizer.DbModel.User;

namespace TgBot.BotCommands
{
    public abstract class BotCommand
    {
        public abstract string Name { get; }
        protected readonly ChatController chat;

        protected BotCommand(ChatController chatController) => chat = chatController;

        public abstract Task<bool> Execute(User user, Message message, params string[] param);

        public abstract Task<bool> Next(User user, Message message);
    }
}
