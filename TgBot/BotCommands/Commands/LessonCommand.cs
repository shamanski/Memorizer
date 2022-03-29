using ReversoConsole.Algorithm;
using ReversoConsole.Controller;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Keybords;

namespace TgBot.BotCommands.Commands
{
    public class LessonCommand : BotCommand
    {
        private const string positive = "\u2705";
        private const string negative = "\u274C";
        public override string Name { get; } = "/lesson";
        private StandardLesson lessonController;
        private Lesson lesson;
        int count = 0;

        public LessonCommand(ChatController chatController) : base(chatController)
        {
        }

        public async override Task<bool> Execute(ReversoConsole.DbModel.User user, Message message)
        {
            var learning = new LearningController(user);
            lessonController = new StandardLesson(user);
            if (learning.GetAll().Count < 10)
            {
                message.Text = "Добавьте хотя бы 10 слов для изучения. Можно использовать /startpack";
                await chat.ReplyMessage(message);
                return false;
            } 
            lesson = lessonController.GetNextLesson();
            await StepAnswer(message);
            return  true;
        }

        private async Task StepAnswer(Message message)
        {
            var currentWord = lesson.WordsList[count].LearningWord.WordToLearn;
            var translates = String.Join(", ", currentWord.Translates.Take(10));
            message.Text = translates;
            var w = lesson.WordsList[count].AdditionalWords;
            var rnd = new Random(); 
            w.Insert(rnd.Next(w.Count), lesson.WordsList[count].LearningWord.WordToLearn.Text);
            message.ReplyMarkup = new LessonKeyboard(w.ToArray(), currentWord.Text).Keyboard;
            await chat.ReplyMessage(message);  
        }
        
        private async Task CheckBox(Message message, string currWord)
        {
            if (message.ReplyMarkup != null )
            {
                foreach (var i in message?.ReplyMarkup?.InlineKeyboard)
                {
                    foreach (var n in i)
                    {
                        if (!n.CallbackData.StartsWith('/'))
                        {
                            n.Text = n.Text.Remove(0, 1).Insert(0, currWord == n.Text[1..] 
                                ? positive 
                                : negative);
                        }
                    }
                }
                await chat.EditMessageAsync(message);
            }

            else
            {
                message.Text = currWord == message.Text ? message.Text.Insert(0, positive) : message.Text.Insert(0, negative);
                await chat.ReplyMessage(message);
            }
            
        }

        private string Clear(string input)
        {
            var w = input;
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
            if (!char.IsLetter(input[0])) w = input[1..];
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(w);
        } 
        
        public async override Task<bool> Next(ReversoConsole.DbModel.User user, Message message)
        {
            var currWord = lesson.WordsList[count].ToString();
            if (message.Text == "!rm")
            {
                lesson.WordsList[count].IsSuccessful = IsSuccessful.Finished;
            }
            else
            {
                message.Text = Clear(message.Text);
                lesson.WordsList[count].IsSuccessful = (currWord == message.Text) ? IsSuccessful.True : IsSuccessful.False;
            }
            
            count++;
            await CheckBox(message, currWord); 
            if (count == lesson.WordsList.Count)
            {
                await Last(message);
                return false;
            }

            await StepAnswer(message);
            return true;
        }

        public async Task Last(Message message)
        {           
            lessonController.ReturnFinishedLesson(lesson);
            int successful = (from i in lesson.WordsList
                              where i.IsSuccessful == IsSuccessful.True || i.IsSuccessful == IsSuccessful.Finished
                              select i).Count();
            message.Text = $"Урок окончен. Результат: { successful} из {lesson.WordsList.Count}";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);
        }
    }
}
