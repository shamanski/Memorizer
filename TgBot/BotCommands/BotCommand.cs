using System.Threading.Tasks;
using Telegram.Bot.Types;
using Memorizer.DbModel;
using User = Memorizer.DbModel.User;
using Memorizer.Controller;

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
