﻿using Memorizer.Algorithm;
using Memorizer.Controller;
using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Memorizer.ConsoleCommands.Commands
{
    class LessonCommand : ConsoleCommand
    {
        public override string Name { get; } = "Start new lesson";

        public override bool Contains(string message)
        {
            throw new NotImplementedException();
        }

        public override Task Execute(User user, IEnumerable<string> arguments)
        {
            var lessonController = new StandardLesson(user, new WebAppContext());
            var lesson = lessonController.GetNextLesson(new WebAppContext());
            foreach (var currWord in lesson.WordsList)
            {
                Console.WriteLine(currWord.LearningWord.WordToLearn.Translates[0].Text);
                if (Console.ReadLine() == currWord.LearningWord.WordToLearn.Text)
                {
                    currWord.IsSuccessful = IsSuccessful.True;
                    Console.WriteLine("Yes");
                }
                else
                {
                    currWord.IsSuccessful = IsSuccessful.False;
                    Console.WriteLine("No");
                }

            }
            lessonController.ReturnFinishedLesson(lesson, new WebAppContext());
            return Task.CompletedTask;
        }
    }
}
