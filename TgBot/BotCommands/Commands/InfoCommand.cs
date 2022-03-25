using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.BotCommands.Commands
{
    class InfoCommand : BotCommand
    {
        public override string Name => throw new NotImplementedException();

        public async override Task<bool> Execute(User user, Telegram.Bot.Types.Message message)
        {
            var learningController = new LearningController(user);
            var i = learningController.GetAll();
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
            await ChatController.ReplyMessage(message);
            return false;
        }

        public override Task<bool> Next(User user, Telegram.Bot.Types.Message message)
        {
            throw new NotImplementedException();
        }
    }
}
