using ReversoConsole.Controller;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBot.BotCommands;
using TgBot.Keybords;

namespace TgBot
{
    static class Program
    {
        private static string token = "5065451258:AAF73Y8um3nU30RXEPQ59kVq98XnUTYYfeg";
        public static CancellationTokenSource Cts { get; set; }
        
        static void Main(string[] args)
        {
            Cts = new CancellationTokenSource();
            ChatController.SetBot(token);
            Console.WriteLine("Bot started...");
            Console.ReadKey();
        }
    }
}
