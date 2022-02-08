using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands
{
    interface ICommandService
    {
        Dictionary<string, BotCommand> Get();
        bool Execute(ReversoConsole.DbModel.User user, Message message);
    }
}
