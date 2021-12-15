using ReversoConsole.ConsoleCommands.Commands;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TgBot.BotCommands.Commands;

namespace TgBot.BotCommands
{
    class CommandService : ICommandService
    {
        private readonly Dictionary<string, BotCommand> _commands;

        public CommandService()
        {
            _commands = new Dictionary<string, BotCommand>
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

        public Dictionary<string, BotCommand> Get() => _commands;
        public Task Execute(User user, string message)
        {
            
            var split = message.Split(' ');
            try
            {
                return _commands[split.First()].Execute(user, split.Skip(1));
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
