using Model.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.BotCommands.Commands;
using User = Memorizer.DbModel.User;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TgBot.BotCommands
{
    public class CommandService : ICommandService
    {
        private readonly StateController<BotCommand> states;
        private readonly ChatController chat;
        private readonly IServiceProvider _provider;
        private readonly WebAppContext _context;
        private readonly AllWordsService _allWords;
        private List<BotCommand> _commands;

        public CommandService(StateController<BotCommand> state, ChatController chatController, AllWordsService allWords, WebAppContext context, IServiceProvider provider)
        {
            _provider = provider;
            _allWords = allWords;
            states = state;
            _context = context;
            chat = chatController;  
            Refresh(); 
        }

        private void Refresh()
        {
           _commands = _provider.GetServices<BotCommand>().ToList();
  
        }

        public List<BotCommand> Get() => _commands;

        public async Task<bool> Execute(User user, Message message)
        {
            var state = states.GetUserState(user.Name);
            string[] split = { };
            try
            {
                
                if (message.Text != null && message.Text.StartsWith('/')) // Got a command
                {
                    split = message.Text?.Split(' ');
                    states.RemoveUserState(user.Name);  //Stop current command if exists
                    var hasNext = await _commands.FirstOrDefault(i => i.Name == split.First()).Execute(user, _context, message, split[1..]); //Try to execute new command
                    if (hasNext) // Command should continue
                    {
                        states.Add(user.Name, _commands.First(i => i.Name == split.First())); //Save command
                    }
                    return hasNext; //True if command has next steps
                }

                else if (state != null) // If request is not a command and saved command exists
                {
                    if ( !await state.Next(user, _context, message)) //Execute saved command and check if it should continue
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
