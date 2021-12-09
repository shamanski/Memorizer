using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands
{
    class HelpCommand : ConsoleCommand
    {
        public override string Name { get; } = "help";

        public override bool Contains(string message)
        {
            return message.Contains(Name);
        }

        public override Task Execute(User user, string message)
        {
            throw new NotImplementedException();
        }
    }
}
