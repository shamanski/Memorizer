using Memorizer.DbModel;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System;
using ReversoApi;
using System.Net;
using Memorizer.Controller;
using Memorizer.DbModel;
using System.Collections.Generic;

namespace TgBot.BotCommands.Commands
{

    [Command(Description = "/load - Загрузка слов из файла")]
    public class LoadFileCommand : BotCommand
    {
        private readonly ChatController chatController;
        private readonly AllWordsController allWords;
        public override string Name { get; } = "/load";

        public LoadFileCommand(ChatController chatController, AllWordsController allWords) : base(chatController)
        {
            this.chatController = chatController;
            this.allWords = allWords;
        }

        public async override Task<bool> Execute(User user, WebAppContext context, Telegram.Bot.Types.Message message, params string[] param)
        {
            message.Text = $"Отправьте файл txt";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);
            return true;
        }

        public async override Task<bool> Next(User user, WebAppContext context, Telegram.Bot.Types.Message message)
        {
            var learningController = new LearningController(user, chat.GetContext);
            if (message.Type == MessageType.Document)
            {
                using var client = new WebClient();
                var file = chatController.GetFile(message);
                var myDataBuffer = client.DownloadString(file);
                var split = myDataBuffer.Split(Environment.NewLine);
                var wordsToAdd = new List<Word>();

                foreach (var i in split)
                {
                    var w = await allWords.FindWordByName(i);
                    if (w != null) wordsToAdd.Add(w);
                }

                message.Text = $"Добавлено {learningController.AddNewWords(wordsToAdd)} слов";
                await chat.ReplyMessage(message);
            }
            return false;
        }
    }
}
