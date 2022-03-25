using ReversoConsole.Algorithm;
using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands.Commands
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
            var lessonController = new StandardLesson(user);
            var lesson = lessonController.GetNextLesson();
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
            lessonController.ReturnFinishedLesson(lesson);
            return Task.CompletedTask;
        }
    }
}
