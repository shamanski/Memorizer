using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands.Commands
{
    class RemoveCommand : ConsoleCommand
    {
        public override string Name { get; } = "Remove word(s) from your list";

        public override bool Contains(string message)
        {
            throw new NotImplementedException();
        }

        public override Task Execute(User user, IEnumerable<string> message)
        {
            var allWords = new AllWordsController();
            var learningController = new LearningController(user);
            if ((message.Any() == false) || (message == null))
            {
                Console.Write("Type the word:");
                message = new string[] { Console.ReadLine() };
            }

            foreach (var word in message)
            {
                var rmWord = learningController.Find(word); 

                if (learningController.RemoveWord(rmWord)) 
                {
                    Console.WriteLine($"Word {rmWord.WordToLearn.Text} removed.");
                } 
            }

            foreach (var item in learningController.Words)
            {
                Console.WriteLine($"\t{item.WordToLearn.Text} - {item.WordToLearn.Translates[0].Text}");
            }

            return Task.CompletedTask;
        }
    }
}
