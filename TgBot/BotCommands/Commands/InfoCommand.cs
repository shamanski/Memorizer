using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/info - Статистика")]
    public class InfoCommand : BotCommand
    {
        public InfoCommand(ChatController chatController) : base(chatController)
        {
        }

        public override string Name { get; } = "/info";

        public async override Task<bool> Execute(IUser user, Telegram.Bot.Types.Message message, params string[] param)
        {
            var learningController = new LearningController(user, new WebAppContext());
            var i = learningController
                .GetAll().OrderByDescending(i => i.Level)
                .ToList();
            var str = new StringBuilder();
            foreach (var word in i)
            {
                if (word.Level == -1)
                {
                    str.Append(Enumerable.Repeat('\u25CF', 8).ToArray());
                }
                else
                {
                    str.Append(Enumerable.Repeat('\u25CF', word.Level).ToArray());
                    str.Append(Enumerable.Repeat('\u25CB', 8 - word.Level).ToArray());
                }
                
                str.Append($"   {word} - {word.WordToLearn.Translates[0].Text}".PadRight(30));               
                str.AppendLine();
            }
            str.AppendLine($"Всего {i.Count} слов");
            str.AppendLine($"Изучено {i.Count(word => word.Level == -1)} слов");


            message.Text = str.ToString();
            await chat.ReplyMessage(message);
            return false;
        }

        public override Task<bool> Next(IUser user, Telegram.Bot.Types.Message message)
        {
            throw new NotImplementedException();
        }
    }
}
