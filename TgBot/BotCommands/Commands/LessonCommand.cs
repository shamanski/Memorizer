﻿using ReversoConsole.Algorithm;
using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.BotCommands.Commands
{
    class LessonCommand : BotCommand
    {
        public override string Name { get; } = "Start new lesson";

        public override bool Contains(string message)
        {
            throw new NotImplementedException();
        }

        public override Task Execute(User user, IEnumerable<string> message)
        {
            var learning = new LearningController(user);
            var lesson = learning.Lesson.GetNextLesson();
            foreach (var currWord in lesson.WordsList)
            {
                Console.WriteLine(currWord.LearningWord.WordToLearn.Translates[0].Text);
                if (Console.ReadLine() == currWord.LearningWord.WordToLearn.Text)
                {
                    currWord.isSuccessful = IsSuccessful.True;
                    Console.WriteLine("Yes");
                }
                else
                {
                    currWord.isSuccessful = IsSuccessful.False;
                    Console.WriteLine("No");
                }

            }
            learning.Lesson.ReturnFinishedLesson(lesson);
            return Task.CompletedTask;
        }
    }
}
