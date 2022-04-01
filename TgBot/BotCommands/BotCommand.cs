using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands
{
    public abstract class BotCommand
    {
        public abstract string Name { get; }
        protected readonly ServiceProvider _services;

        protected BotCommand(ServiceProvider services) => _services = services;

        public abstract Task<bool> Execute(ReversoConsole.DbModel.User user, Message message, params string[] param);

        public abstract Task<bool> Next(ReversoConsole.DbModel.User user, Message message);
    }
}
