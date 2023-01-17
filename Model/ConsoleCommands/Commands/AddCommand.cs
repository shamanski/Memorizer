using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands.Commands
{
    class AddCommand : ConsoleCommand
    {
        public override string Name { get; } = "Add new word(s) to your list";

        public override bool Contains(string message)
        {
            throw new NotImplementedException();
        }

        public override Task Execute(User user, IEnumerable<string> message)
        {
            var allWords = new AllWordsController(new WebAppContext());
            var learningController = new LearningController(user, new WebAppContext());
            if ((message.Any() == false) || (message == null))
            {
                Console.Write("Type the word:");
                message = new string[] { Console.ReadLine() };
            }
            foreach (var word in message )
            {
                var makeWord = allWords.FindWordByName(word);
                var newWord = new LearningWord(user, makeWord);
                learningController.AddNewWord(newWord);
            }

            foreach (var item in learningController.GetAll())
            {
                Console.WriteLine($"\t{item.WordToLearn.Text} - {item.WordToLearn.Translates[0].Text}");
            }

            return Task.CompletedTask;
        }
    }
}
