using Model.Services;
using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/info - Статистика")]
    public class InfoCommand : BotCommand, IBotCommand
    {
        public InfoCommand(ChatController chatController, LearningService learning) : base(chatController)
        {
        }

        private readonly LearningService learning;
        public override string Name { get; } = "/info";

        public async override Task<bool> Execute(User user, Telegram.Bot.Types.Message message, params string[] param)
        {
            var str = new StringBuilder();
            /* var learningController = new LearningService(user, context);
             var i = learning
                 .GetAll().OrderByDescending(i => i.Level)
                 .ToList();

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
             } */
            str.AppendLine($"Всего {learning.Count()} слов");
            str.AppendLine($"Изучено {learning.GetLearnedWords()} слов");


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
