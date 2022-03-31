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
        
        public async override Task<bool> Execute(ReversoConsole.DbModel.User user, Message message, params string[] param)
        {
            LearningController learningController = new LearningController(user);
            AllWordsController allWords = new AllWordsController();
            int start = default, count = default;
            switch (param.Length)
            {
                case 0:
                    {
                        return false;
                    }
                case 1:
                    {
                        int.TryParse(param[0], out start);
                        count = 1;
                        break;
                    }
                case 2:
                    {
                        int.TryParse(param[0], out start);
                        int.TryParse(param[1], out count);
                        break;
                    }
                    default: return false;
            }
            try
            {
                var wordsToAdd = allWords.FindWordsById(start, count);
                
                message.Text = $"Добавлено {learningController.AddNewWords(wordsToAdd)} слов";
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
