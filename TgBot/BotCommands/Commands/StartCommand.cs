using ReversoConsole.Controller;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands.Commands
{
    public class StartCommand : BotCommand
    {
        public StartCommand(ChatController chatController) : base(chatController)
        {
        }

        public override string Name { get; } = "/startpack";     
        
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
            await chat.ReplyMessage(message);
            return false;
        }

        public async override Task<bool> Next(ReversoConsole.DbModel.User user, Message message)
        {
            await Task.FromResult(true);
            return false;
        }
    }
}
