using Memorizer.ConsoleCommands.Commands;
using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Memorizer.ConsoleCommands
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

        public Task Execute(User user, string message)
        {
            var split = Regex.Split(message, @"\s+").Where(s => s != string.Empty);
            try
            {
                _commands[split.First()].Execute(user, split.Skip(1));
                    return Task.CompletedTask;
            }

            catch (KeyNotFoundException)
            {
                System.Console.WriteLine("Command doesn't exists");
                return Task.CompletedTask;
            }

            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
        }
    }
}
