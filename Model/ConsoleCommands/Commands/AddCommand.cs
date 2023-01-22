using Memorizer.Controller;
using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorizer.ConsoleCommands.Commands
{
    class AddCommand : ConsoleCommand
    {
        public override string Name { get; } = "Add new word(s) to your list";

        public override bool Contains(string message)
        {
            throw new NotImplementedException();
        }

        public async override Task Execute(User user, IEnumerable<string> message)
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
                var makeWord = await allWords.FindWordByName(word);
                var newWord = new LearningWord(user, makeWord);
                learningController.AddNewWord(newWord);
            }

            foreach (var item in learningController.GetAll())
            {
                Console.WriteLine($"\t{item.WordToLearn.Text} - {item.WordToLearn.Translates[0].Text}");
            }
        }
    }
}
