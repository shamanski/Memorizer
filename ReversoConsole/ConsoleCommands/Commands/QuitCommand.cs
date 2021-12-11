using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands.Commands
{
    class QuitCommand : ConsoleCommand
    {
        public override string Name { get; } = "Q";

        public override bool Contains(string message)
        {
            throw new NotImplementedException();
        }

        public override Task Execute(User user, IEnumerable<string> message)
        {
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}
