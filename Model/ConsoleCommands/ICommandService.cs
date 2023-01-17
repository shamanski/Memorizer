using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands
{
    interface ICommandService
    {
        Dictionary<string, ConsoleCommand> Get();
        Task Execute(User user, string message);
    }
}
