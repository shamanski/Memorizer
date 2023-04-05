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
using System.Windows.Input;
using ReversoApi.Models;

namespace TgBot.BotCommands
{
    public class CommandService : ICommandService
    {
        private readonly StateController<BotCommand> states;
        private readonly ChatController chat;
        private readonly IServiceProvider provider;
        private readonly AllWordsService allWords;
        private readonly IEnumerable<IBotCommand> commands;

        public CommandService(StateController<BotCommand> state, ChatController chatController, AllWordsService allWords, IEnumerable<IBotCommand> commands, IServiceProvider provider)
        {
            this.provider = provider;
            this.allWords = allWords;
            this.states = state;
            this.chat = chatController;
            this.commands = commands;
            states = state;
            chat = chatController;  

        }

        public List<IBotCommand> Get() => commands.ToList();

        public async Task<bool> Execute(User user, Message message)
        {
            var state = await states.GetUserState(user);
            string[] split = { };
            try
            {
                
                if (message.Text != null && message.Text.StartsWith('/')) // Got a command
                {
                    split = message.Text?.Split(' ');
                    var t = Get();
                    await states.RemoveUserState(user.Id);  //Stop current command if exists
                    var command = commands.FirstOrDefault(c => c.Name == split.First());                    
                    if (command != null)
                    {
                        var hasNext = await command.Execute(user, message, split[1..]);
                        if (hasNext)
                        {
                            await states.Add(user.Id, (BotCommand)command);
                        }
                        return hasNext;
                    }
                    else
                    {
                        System.Console.WriteLine($"Command '{split.First()}' doesn't exist");
                    }
                }

                else if (state != null) // If request is not a command and saved command exists
                {
                    if ( !await state.Next(user, message)) //Execute saved command and check if it should continue
                    {
                        await states.RemoveUserState(user.Id);
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
