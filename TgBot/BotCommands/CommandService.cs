using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.BotCommands.Commands;

namespace TgBot.BotCommands
{
    public class CommandService : ICommandService
    {
        private readonly StateController states;
        private readonly ChatController chat;
        private readonly AllWordsController _allWords;
        private List<BotCommand> _commands;

        public CommandService(StateController state, ChatController chatController, AllWordsController allWords)
        {
            _allWords = allWords;
            states = state;
            chat = chatController;  
            Refresh(); 
        }

        private void Refresh()
        {
            _commands = new List<BotCommand>
            {
                {new LessonCommand(chat) },
                {new AddCommand(chat) },
                { new RemoveCommand(chat) },
                { new InfoCommand(chat) },
                { new RemoveCommand(chat) },
                { new RemoveCommand(chat) },
                { new HelpCommand(chat) },
                { new LoadFileCommand(chat, _allWords) },
                {new StartCommand(chat) }
            };
        }

        public List<BotCommand> Get() => _commands;

        public async Task<bool> Execute(IUser user, Message message)
        {
            var state = states.GetUserState(user.Name);
            string[] split = { };
            try
            {
                
                if (message.Text != null && message.Text.StartsWith('/')) // Got a command
                {
                    split = message.Text?.Split(' ');
                    states.RemoveUserState(user.Name);  //Stop current command if exists
                    var res = _commands.FirstOrDefault(i => i.Name == split.First()).Execute(user, message, split[1..]).Result; //Try to execute new command
                    if (res) // Command should continue
                    {
                        states.Add(user.Name, _commands.First(i => i.Name == split.First())); //Save command
                    }
                    return res; //True if command has next steps
                }

                else if (state != null) // If request is not a command and saved command exists
                {
                    if (!state.Next(user, message).Result) //Execute saved command and check if it should continue
                    {
                        states.RemoveUserState(user.Name);
                        Refresh();
                    }
                    return false; // Nothing to do
                }
            }            
            catch (KeyNotFoundException) // Command not founded
            {
                System.Console.WriteLine($"Command doesn't exists");
                return false;
            }

            return false; // If request is not a command and saved command doesn't exists
        }

    }
}
