using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReversoConsole.ConsoleCommands
{
    class HelpCommand : ConsoleCommand
    {
        public override string Name { get; } = "Help";

        public override bool Contains(string message)
        {
            return message.Contains(Name);
        }

        public override Task Execute(User user, IEnumerable<string> arguments)
        {
            Console.WriteLine("Что вы хотите сделать?");
            Console.WriteLine("E - ввести слово");
            Console.WriteLine("A - урок");
            Console.WriteLine("Q - выход");
            return Task.CompletedTask;
        }
    }
}
