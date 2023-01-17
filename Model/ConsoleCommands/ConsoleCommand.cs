using Memorizer.DbModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Memorizer.ConsoleCommands
{
    public abstract class ConsoleCommand
    {
        public abstract string Name { get; }

        public abstract Task Execute(User user, IEnumerable<string> arguments);

        public abstract bool Contains(string message);
    }
}
