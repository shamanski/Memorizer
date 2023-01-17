using Memorizer.Controller;
using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Memorizer.ConsoleCommands.Commands
{
    class InfoCommand : ConsoleCommand
    {
        public override string Name { get; } = "Show list of your words";

        public override bool Contains(string message)
        {
            throw new NotImplementedException();
        }
        public override Task Execute(User user, IEnumerable<string> message)
        {
            var learningController = new LearningController(user, new WebAppContext());

            foreach (var item in learningController.GetAll())
            {
                Console.WriteLine($"\t{item.WordToLearn.Text} - {item.WordToLearn.Translates[0].Text}");
            }

            return Task.CompletedTask;
        }
    }
}
