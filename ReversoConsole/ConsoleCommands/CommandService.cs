using ReversoConsole.ConsoleCommands.Commands;
using ReversoConsole.DbModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands
{
    class CommandService : ICommandService
    {
        private readonly Dictionary<string, ConsoleCommand> _commands;

        public CommandService()
        {
            _commands = new Dictionary<string, ConsoleCommand>
            {
                { "H", new HelpCommand() },
                { "L", new LessonCommand() },
                { "A", new AddCommand() },
               // new RemoveCommand(),
               // new StatCommand()
               { "Q", new QuitCommand() }
            };
        }

        public Dictionary<string, ConsoleCommand> Get() => _commands;
        public Task Execute(User user, string message)
        {
            var split = Regex.Split(message, @"\s+").Where(s => s != string.Empty);
            try
            {
                return _commands[split.First()].Execute(user, split.Skip(1));
            }
            catch
            {
                System.Console.WriteLine("Command doesn't exists");
                return Task.CompletedTask;
            }
        }
    }
}
