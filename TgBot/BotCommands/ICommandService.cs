using ReversoConsole.DbModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands
{
    public interface ICommandService
    {
        public List<BotCommand> Get();
        public Task<bool> Execute(IUser user, Message message);
    }
}
