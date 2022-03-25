using ReversoConsole.Algorithm;
using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Keybords;

namespace TgBot.BotCommands.Commands
{
    class LessonCommand : BotCommand
    {
        public override string Name { get; } = "Start new lesson";
        private StandardLesson lessonController;
        private Lesson lesson;
        int count = 0;
        
        public async override Task<bool> Execute(ReversoConsole.DbModel.User user, Message message)
        {
            var learning = new LearningController(user);
            lessonController = new StandardLesson(user);
            if (learning.GetAll().Count < 10)
            {
                message.Text = "Добавьте хотя бы 10 слов для изучения. Можно использовать /startpack";
                await ChatController.ReplyMessage(message);
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
            w.Add(lesson.WordsList[count].LearningWord.WordToLearn.Text);
            message.ReplyMarkup = new LessonKeyboard(w.ToArray(), currentWord.Text).Keyboard;
            await ChatController.ReplyMessage(message);  
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
                                ? '\u2705'.ToString() 
                                : '\u274C'.ToString());
                        }
                    }
                }
                await ChatController.EditMessageAsync(message);
            }

            else
            {
                message.Text += currWord == message.Text ? '\u2713' : '\u274C';
                await ChatController.ReplyMessage(message);
            }
            
        }

        public async override Task<bool> Next(ReversoConsole.DbModel.User user, Message message)
        {
            var currWord = lesson.WordsList[count].ToString();
            if (message.Text == "/rm")
            {
                lesson.WordsList[count].IsSuccessful = IsSuccessful.Finished;
            }
            else
            {
                lesson.WordsList[count].IsSuccessful = (currWord == message.Text[1..]) ? IsSuccessful.True : IsSuccessful.False;
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
            await ChatController.ReplyMessage(message);
        }
    }
}
