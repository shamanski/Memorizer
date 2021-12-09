using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands
{
    public abstract class ConsoleCommand
    {
        public abstract string Name { get; }

        public abstract Task Execute(User user, string message);

        public abstract bool Contains(string message);
    }
}
