using Memorizer.DbModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using TgBot.BotCommands.Commands;

namespace TgBot.BotCommands
{
    public interface ICommandService
    {
        public List<IBotCommand> Get();
        public Task<bool> Execute(User user, Telegram.Bot.Types.Message message);
    }
}
