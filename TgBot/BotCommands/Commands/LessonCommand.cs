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
        private Lesson lesson;
        int count = 0;
        public override Task Execute(ReversoConsole.DbModel.User user, Message message)
        {
            learning = new LearningController(user);
            lesson = learning.Lesson.GetNextLesson();
            var translates = String.Join(',', lesson.WordsList[0].LearningWord.WordToLearn.Translates);
            message.Text = translates;
            var w = lesson.WordsList[count].AdditionalWords;
            w.Add(lesson.WordsList[0].LearningWord.WordToLearn.Text);
            message.ReplyMarkup = new LessonKeyboard(w.ToArray()).Keyboard;
            ChatController.ReplyMessage(message);
            return Task.CompletedTask;
        }

        public override bool Next(ReversoConsole.DbModel.User user, Message message)
        {
            var currWord = lesson.WordsList[count].LearningWord.WordToLearn.Text; 
            lesson.WordsList[count].isSuccessful = (currWord == message.Text) ? IsSuccessful.True : IsSuccessful.False;
            count++;
            foreach (var i in message?.ReplyMarkup?.InlineKeyboard)
            {
                foreach (var n in i)
                {
                    n.Text = currWord == n.Text ? n.Text += '\u2713'.ToString() : '\u274C'.ToString();                    
                }
            }
            ChatController.EditMessage(message);
            if (count == lesson.WordsList.Count)
            {
                learning.Lesson.ReturnFinishedLesson(lesson);
                int successful = 0;
                foreach (var i in lesson.WordsList)
                {
                    if (i.isSuccessful == IsSuccessful.True) successful++;
                }
                message.Text = $"Урок окончен. Результат: { successful} из {lesson.WordsList.Count}";
                message.ReplyMarkup = null;
                ChatController.ReplyMessage(message);
                return false;
            }
            var translates = String.Join(',', lesson.WordsList[count].LearningWord.WordToLearn.Translates);
            message.Text = translates;
            var w = lesson.WordsList[count].AdditionalWords;
            w.Add(lesson.WordsList[count].LearningWord.WordToLearn.Text);
            message.ReplyMarkup = new LessonKeyboard(w.ToArray()).Keyboard;
            ChatController.ReplyMessage(message);
            return true;
        }
    }
}
