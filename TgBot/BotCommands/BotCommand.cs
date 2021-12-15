using ReversoConsole.DbModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.BotCommands
{
    public abstract class BotCommand
    {
        public abstract string Name { get; }

        public abstract Task Execute(User user, IEnumerable<string> arguments);

        public abstract bool Contains(string message);
    }
}
