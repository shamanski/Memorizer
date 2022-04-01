using ReversoConsole.DbModel;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System;
using System.Net;
using ReversoConsole.Controller;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/load - Загрузка файла со словами")]
    public class LoadFileCommand : BotCommand
    {
        private readonly ChatController chat;
        public override string Name { get; } = "/load";

        public LoadFileCommand(ServiceProvider services) : base(services)
        {
            chat = services.GetRequiredService<ChatController>();
        }

        public async override Task<bool> Execute(User user, Telegram.Bot.Types.Message message, params string[] param)
        {
            message.Text = $"Отправьте файл txt";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);
            return true;
        }

        public async override Task<bool> Next(User user, Telegram.Bot.Types.Message message)
        {
            var allWords = new AllWordsController();
            var learningController = new LearningController(user);
            if (message.Type == MessageType.Document)
            {
                using var client = new WebClient();
                var file = chat.GetFile(message);
                var myDataBuffer = client.DownloadString(file);
                var split = myDataBuffer.Split(Environment.NewLine);
                var wordsToAdd = new List<Word>();

                    foreach (var i in split)
                    {
                    try
                    {
                        wordsToAdd.Add(allWords.FindWordByName(i));
                    }
                    catch
                    {
                        
                    }
                    }

                message.Text = $"Добавлено {learningController.AddNewWords(wordsToAdd)} слов";
                await chat.ReplyMessage(message);
            }
            return false;
        }
    }
}
