using ReversoConsole.DbModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TgBot.BotCommands
{
    public interface ICommandService
    {
        public List<BotCommand> Get();
        public Task<bool> Execute(User user, Telegram.Bot.Types.Message message);
    }
}
