using ReversoConsole.DbModel;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TgBot.BotCommands
{
    public interface ICommandService
    {
        List<BotCommand> Get();
        bool Execute(IUser user, Message message);
    }
}
