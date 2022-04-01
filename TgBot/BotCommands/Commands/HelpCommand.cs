using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TgBot.BotCommands.Commands
{
    public class HelpCommand : BotCommand
    {
        private readonly ChatController chat;

        public HelpCommand(ServiceProvider services) : base(services)
        {
            chat = services.GetRequiredService<ChatController>();
        }

        public override string Name { get; } = "/help";

        public async override Task<bool> Execute(User user, Telegram.Bot.Types.Message message, params string[] param)
        {
            var str = new StringBuilder();
            foreach (var typeList in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeList.GetCustomAttribute<CommandAttribute>() != null)
                {
                    str.AppendLine(typeList.GetCustomAttribute<CommandAttribute>().Description);
                }
            }
            message.Text = str.ToString();
            await chat.ReplyMessage(message);
            return false;
        }

        public override Task<bool> Next(User user, Telegram.Bot.Types.Message message)
        {
            throw new NotImplementedException();
        }
    }
}

