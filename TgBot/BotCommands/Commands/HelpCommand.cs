using Model.Services;
using Memorizer.DbModel;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.BotCommands.Commands
{
    public class HelpCommand : BotCommand, IBotCommand
    {
        public HelpCommand(ChatController chatController) : base(chatController)
        {
        }

        public override string Name { get; } = "/help";

        public async override Task<bool> Execute(User user, Telegram.Bot.Types.Message message, params string[] param)
        {
            var str = new StringBuilder();
            foreach (var attr in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (attr.GetCustomAttribute<CommandAttribute>() != null)
                {
                    str.AppendLine(attr.GetCustomAttribute<CommandAttribute>().Description);
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

