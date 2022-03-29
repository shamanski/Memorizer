using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TgBot.BotCommands
{
    public interface ICommandService
    {
        List<BotCommand> Get();
        bool Execute(ReversoConsole.DbModel.User user, Message message);
    }
}
