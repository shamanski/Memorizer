using ReversoConsole.ConsoleCommands.Commands;
using ReversoConsole.DbModel;
using System;
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
                { "R", new RemoveCommand() },
                { "I", new InfoCommand() },
               // new StatCommand()
               { "Q", new QuitCommand() }
            };
        }

        public Dictionary<string, ConsoleCommand> Get() => _commands;

        public async Task<bool> Execute(IUser user, string message)
        {
            var split = Regex.Split(message, @"\s+").Where(s => s != string.Empty);
            try
            {
                return await _commands[split.First()].Execute(user, split.Skip(1));
            }

            catch (KeyNotFoundException)
            {
                System.Console.WriteLine("Command doesn't exists");
                return true;
            }

            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return true;
            }
        }
    }
}
