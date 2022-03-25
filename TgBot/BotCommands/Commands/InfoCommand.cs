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
        private LearningController learningController;
        public override string Name => throw new NotImplementedException();

        public override bool Execute(User user, Telegram.Bot.Types.Message message)
        {
            learningController = new LearningController(user);
            var i = learningController.GetAll();
            var str = new StringBuilder();
            foreach (var word in i)
            {
                str.Append(Enumerable.Repeat('\u25CF', word.Level).ToArray());
                str.Append(Enumerable.Repeat('\u25CB', 7 - word.Level).ToArray());
                str.Append($"   {word.ToString()} - {word.WordToLearn.Translates[0].Text}".PadRight(30));               
                str.AppendLine();
            }
            str.Append($"Всего {i.Count} слов");
            
            message.Text = str.ToString();
            ChatController.ReplyMessage(message);
            return false;
        }

        public override bool Next(User user, Telegram.Bot.Types.Message message)
        {
            throw new NotImplementedException();
        }
    }
}
