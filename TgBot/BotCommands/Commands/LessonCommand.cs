using ReversoConsole.Algorithm;
using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Keybords;

namespace TgBot.BotCommands.Commands
{
    class LessonCommand : BotCommand
    {
        public override string Name { get; } = "Start new lesson";
        private LearningController learning;
        private StandardLesson lessonController;
        private Lesson lesson;
        int count = 0;
        public override bool Execute(ReversoConsole.DbModel.User user, Message message)
        {
            learning = new LearningController(user);
            lessonController = new StandardLesson(user);
            lesson = lessonController.GetNextLesson();
            StepAnswer(message);
            return  true;
        }

        private void StepAnswer(Message message)
        {
            var translates = String.Join(',', lesson.WordsList[count].LearningWord.WordToLearn.Translates);
            message.Text = translates;
            var w = lesson.WordsList[count].AdditionalWords;
            w.Add(lesson.WordsList[count].LearningWord.WordToLearn.Text);
            message.ReplyMarkup = new LessonKeyboard(w.ToArray()).Keyboard;
            ChatController.ReplyMessage(message);
        }
        
        private void CheckBox(Message message, string currWord)
        {
            if (message.ReplyMarkup != null )
            {
                foreach (var i in message?.ReplyMarkup?.InlineKeyboard)
                {
                    foreach (var n in i)
                    {
                        n.Text = n.Text.Remove(0,1).Insert(0,  currWord == n.Text.Substring(1) ? '\u2705'.ToString() : '\u274C'.ToString());
                        //if (n.Text.EndsWith('\u2713') || n.Text.EndsWith('\u274C')) return;
                            //n.Text += currWord == n.Text ? '\u2713' : '\u274C';
                    }
                }
                ChatController.EditMessageAsync(message);
            }

            else
            {
                message.Text += currWord == message.Text ? '\u2713' : '\u274C';
                ChatController.ReplyMessage(message);
            }
            
        }

        public override bool Next(ReversoConsole.DbModel.User user, Message message)
        {
            var currWord = lesson.WordsList[count].ToString();
            lesson.WordsList[count].isSuccessful = (currWord == message.Text.Substring(1)) ? IsSuccessful.True : IsSuccessful.False;
            count++;
            CheckBox(message, currWord); 
            if (count == lesson.WordsList.Count)
            {
                Last(message);
                return false;
            }

            StepAnswer(message);
            return true;
        }

        public void Last(Message message)
        {           
            lessonController.ReturnFinishedLesson(lesson);
            int successful = 0;
            foreach (var i in lesson.WordsList)
            {
                if (i.isSuccessful == IsSuccessful.True) successful++;
            }
            message.Text = $"Урок окончен. Результат: { successful} из {lesson.WordsList.Count}";
            message.ReplyMarkup = null;
            ChatController.ReplyMessage(message);
        }
    }
}
