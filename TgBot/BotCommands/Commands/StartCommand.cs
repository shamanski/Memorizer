using ReversoConsole.Controller;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Keybords;

namespace TgBot.BotCommands.Commands
{

    [Command(Description = "/startpack - Добавить набор слов")]
    public class StartCommand : BotCommand
    {
        public StartCommand(ChatController chatController) : base(chatController)
        {
        }

        public override string Name { get; } = "/startpack";

        public async override Task<bool> Execute(ReversoConsole.DbModel.User user, Message message, params string[] param)
        {
            LearningController learningController = new LearningController(user, new WebAppContext());
            AllWordsController allWords = new AllWordsController(new WebAppContext());
            int start = default, count = default;
            switch (param.Length)
            {
                case 0:
                    {
                        message.ReplyMarkup = new AddWordsKeyboard().Keyboard;
                        message.Text = "Наборы слов (от простых к сложным)";
                        await chat.ReplyMessage(message);
                        return false;
                    }
                case 1:
                    {
                        if (!int.TryParse(param[0], out start)) return false;
                        count = 1;
                        break;
                    }
                case 2:
                    {
                        if (!int.TryParse(param[0], out start)) return false;
                        if (!int.TryParse(param[1], out count)) return false;
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
            if (message.ReplyMarkup != null)
            {                
                await chat.CallbackAsync(message);
                message.ReplyMarkup = null;
            }
            
            await chat.ReplyMessage(message);
            return false;
        }

        public async override Task<bool> Next(ReversoConsole.DbModel.User user, Message message)
        {
            message.ReplyMarkup = new AddWordsKeyboard().Keyboard;
            await chat.ReplyMessage(message);
            return false;
        }
    }
}
