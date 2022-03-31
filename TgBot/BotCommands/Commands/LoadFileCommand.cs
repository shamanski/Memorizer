using ReversoConsole.DbModel;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System;
using ReversoApi;

namespace TgBot.BotCommands.Commands
{
    public class LoadFileCommand : BotCommand
    {
        public override string Name { get; } = "/load";

        public LoadFileCommand(ChatController chatController) : base(chatController)
        {

        }

        public async override Task<bool> Execute(User user, Telegram.Bot.Types.Message message, params string[] param)
        {
            message.Text = $"Отправьте файл txt";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);
            return true;
        }

        public async override Task<bool> Next(User user, Telegram.Bot.Types.Message message)
        {
            if (message.Type == MessageType.Document)
            {

                var res = File.ReadLines("")
                         .Where(l => l.Length > 0)
                         .Select(x => x.Split(new[] { ')' }, StringSplitOptions.RemoveEmptyEntries))
                         .Select(i => new WordDescription(i[0], i[1]))
                         .ToList();
            }
            return false;
        }
    }
}
