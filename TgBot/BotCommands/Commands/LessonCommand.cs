using Memorizer.Algorithm;
using Memorizer.Controller;
using Memorizer.DbModel;
using ReversoApi.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Keybords;
using User = Memorizer.DbModel.User;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/lesson - Начать урок")]
    public class LessonCommand : BotCommand
    {
        private const string positive = "\u2705";
        private const string negative = "\u274C";
        public override string Name { get; } = "/lesson";
        private ITakingLesson lessonController;
        private Lesson lesson;
        int count = 0;

        public LessonCommand(ChatController chatController) : base(chatController)
        {
        }

        public async override Task<bool> Execute(User user, WebAppContext context, Message message, params string[] param)
        {
            var learning = new LearningController(user, context);
            lessonController = new StandardLesson(user, context);
            if (learning.GetAll().Count < 10)
            {
                message.Text = "Добавьте хотя бы 10 слов для изучения. Можно использовать /startpack";
                await chat.ReplyMessage(message);
                return false;
            }

            lesson = lessonController.GetNextLesson(context);
            message.Text = "Урок начат"+ Environment.NewLine;            
            await chat.ReplyMessage(message);            
            await StepAnswer(message);
            return  true;
        }

        
        //Take new word and make keyboard
        private async Task StepAnswer(Message message)
        {
            var currentWord = lesson.WordsList[count].LearningWord.WordToLearn;
            message.Text = String.Join(", ", currentWord.Translates.Take(10));
            var additianalList = lesson.WordsList[count].AdditionalWords;
            var rnd = new Random();
            additianalList.Insert(rnd.Next(additianalList.Count), lesson.WordsList[count].LearningWord.WordToLearn.Text);
            message.ReplyMarkup = new LessonKeyboard(additianalList, currentWord.Text).Keyboard;
            await chat.ReplyMessage(message);  
        }
        
        
        //Compare input or button text with correct answer and mark it
        private async Task CheckBox(Message message, string currWord)
        {
            if (message.ReplyMarkup != null ) //button pressed
            {
                foreach (var i in message?.ReplyMarkup?.InlineKeyboard)
                {
                    foreach (var n in i)
                    {
                        if (!n.CallbackData.StartsWith('!')) // excluding special buttons
                        {
                            n.Text = n.Text.Remove(0, 1).Insert(0, currWord == n.Text[1..] 
                                ? positive 
                                : negative);
                        }
                    }
                }
                await chat.EditMessageAsync(message);
            }

            else //answer got as text
            {
                message.Text = currWord == message.Text ? message.Text.Insert(0, positive) : message.Text.Insert(0, negative);
                await chat.ReplyMessage(message);
            }
            
        }

        //Remove special symbols and upper case
        private string Clear(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
            if (!char.IsLetter(input[0])) input = input[1..];
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(input);
        } 
        
        public async override Task<bool> Next(Memorizer.DbModel.User user, WebAppContext context, Message message)
        {
            var currWord = lesson.WordsList[count].ToString();
            if (message.Text == "!rm") // Button 'already have known' pressed
            {
                lesson.WordsList[count].IsSuccessful = IsSuccessful.Finished;
            }
            else
            {
                message.Text = Clear(message.Text);
                lesson.WordsList[count].IsSuccessful = (currWord == message.Text) ? IsSuccessful.True : IsSuccessful.False;
            }
            
            count++;
                       
            if (count == lesson.WordsList.Count) //after last answer
            {
                await Last(user, context, message);
                await CheckBox(message, currWord);
                return false;
            }

            await CheckBox(message, currWord);
            await StepAnswer(message);
            return true;
        }

        private async Task Last(Memorizer.DbModel.User user, WebAppContext context, Message message)
        {
            await lessonController.ReturnFinishedLesson(lesson, context);
            int successful = (from i in lesson.WordsList
                              where i.IsSuccessful == IsSuccessful.True || i.IsSuccessful == IsSuccessful.Finished
                              select i).Count();
            message.Text = $"Урок окончен. Результат: { successful} из {lesson.WordsList.Count}";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);
        }
    }
}
