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
    class StartCommand : BotCommand
    {
        public override string Name { get; } = "Add start words to your list";     
        
        public async override Task<bool> Execute(ReversoConsole.DbModel.User user, Message message)
        {
            LearningController learningController = new LearningController(user);
            AllWordsController allWords = new AllWordsController();
            try
            {
                learningController.AddNewWords(allWords.Words);
                message.Text = $"Добавлено";
            }
            catch
            {
                message.Text = $"Ошибка";
            }
            await ChatController.ReplyMessage(message);
            return false;
        }

        public async override Task<bool> Next(ReversoConsole.DbModel.User user, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
