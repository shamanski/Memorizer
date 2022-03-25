using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands.Commands
{
    class AddCommand : BotCommand
    {
        public override string Name { get; } = "Add new word(s) to your list";
        private LearningController learningController;
        private AllWordsController allWords;

        public async override Task<bool> Execute(ReversoConsole.DbModel.User user, Message message)
        {
            allWords = new AllWordsController();
            learningController = new LearningController(user);
            message.Text = $"Введите слово:";
            message.ReplyMarkup = null;
            await ChatController.ReplyMessage(message);       
            return true;
        }

        public async override Task<bool> Next(ReversoConsole.DbModel.User user, Message message)
        {
            Word makeWord;
            try
            {
                makeWord = allWords.FindWordByName(message.Text);               
            }
            catch
            {
                message.Text = $"Ошибка связи с сервером. Попробуйте позже.";
                await ChatController.ReplyMessage(message);
                return false;
            }

            var newWord = new LearningWord(user, makeWord);
            try
            {
                learningController.AddNewWord(newWord);
                message.Text = $"Добавлено {newWord} {newWord.WordToLearn.Translates[0].Text}";
            }
            catch
            {
                message.Text = $"Уже было добавлено {newWord}";
            }
            await ChatController.ReplyMessage(message);
            return false;
        }
    }
}
