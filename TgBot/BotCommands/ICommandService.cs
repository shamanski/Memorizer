using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.BotCommands
{
    interface ICommandService
    {
        Dictionary<string, BotCommand> Get();
        Task Execute(User user, string message);
    }
}
