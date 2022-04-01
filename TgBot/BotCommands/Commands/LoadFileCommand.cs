using ReversoConsole.DbModel;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System;
using ReversoApi;
using System.Net;
using ReversoConsole.Controller;
using System.Collections.Generic;

namespace TgBot.BotCommands.Commands
{
    public class LoadFileCommand : BotCommand
    {
        private readonly ChatController chatController;
        public override string Name { get; } = "/load";

        public LoadFileCommand(ChatController chatController) : base(chatController)
        {
            this.chatController = chatController;   
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
                var file = chatController.GetFile(message);
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
