using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.ConsoleCommands
{
    interface ICommandService
    {
        List<ConsoleCommand> Get();
    }
}
