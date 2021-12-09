using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.ConsoleCommands
{
    class CommandService : ICommandService
    {
        private readonly List<ConsoleCommand> _commands;

        public CommandService()
        {
            _commands = new List<ConsoleCommand>
            {
                new HelpCommand(),
                new MainCommand(),
                new RankCommand(),
                new ShopCommand(),
                new StartCommand()
            };
        }

        public List<ConsoleCommand> Get() => _commands;
    }
}
