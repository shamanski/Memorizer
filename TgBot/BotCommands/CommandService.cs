using ReversoConsole.ConsoleCommands.Commands;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.BotCommands.Commands;

namespace TgBot.BotCommands
{
    class CommandService : ICommandService
    {

        private BotCommand currentCommand = null;
        private static StateController states;

        private Dictionary<string, BotCommand> _commands;

        public CommandService(StateController state)
        {
            states = state;
            Refresh(); 
        }

        private void Refresh()
        {
            _commands = new Dictionary<string, BotCommand>
            {
               // { "H", new HelpCommand() },
                { "/lesson", new LessonCommand() },
                { "/add", new AddCommand() },
                { "/remove", new RemoveCommand() },
                { "/info", new InfoCommand() },
               // new StatCommand()
              // { "Q", new QuitCommand() }
            };
        }

        public Dictionary<string, BotCommand> Get() => _commands;

        public bool Execute(ReversoConsole.DbModel.User user, Message message)
        {
            var state = states.GetUserState(user.Name);
            if (state != null)
            {
                if (!state.Next(user, message))
                {
                    states.RemoveUserState(user.Name);
                    Refresh();
                }
                    return false;
            }

            else
            {
                var split = message.Text.Split(' ');
                try
                {

                    var res = _commands[split.First()].Execute(user, message);
                    if (res)
                    {
                        states.Add(user.Name, _commands[split.First()]);
                    }
                    
                    return res;

                }

                catch (KeyNotFoundException)
                {
                    if (currentCommand?.Next(user, message) == false) currentCommand = null;
                    System.Console.WriteLine("Command doesn't exists");
                    return false;
                }

            }
           
        }
    }
}
