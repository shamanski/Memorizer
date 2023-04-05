using Memorizer.DbModel;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System;
using System.Net;
using Model.Services;
using System.Collections.Generic;

namespace TgBot.BotCommands.Commands
{

    [Command(Description = "/load - Загрузка слов из файла")]
    public class LoadFileCommand : BotCommand, IBotCommand
    {
        private readonly ChatController chatController;
        private readonly AllWordsService allWords;
        private readonly LearningService learning;
        public override string Name { get; } = "/load";

        public LoadFileCommand(ChatController chatController, AllWordsService allWords, LearningService learnig) : base(chatController)
        {
            this.chatController = chatController;
            this.allWords = allWords;
            this.learning = learnig;
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
            //var learningController = new LearningService(user, chat.GetContext);
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

                message.Text = $"Добавлено {learning.AddNewWords(wordsToAdd)} слов";
                await chat.ReplyMessage(message);
            }
            return false;
        }
    }
}
