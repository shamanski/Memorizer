using ReversoConsole.Controller;
using ReversoConsole.DbModel;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands.Commands
{
    [Command]
    public class AddCommand : BotCommand
    {
        public override string Name { get; } = "/add";
        private LearningController learningController;
        private AllWordsController allWords;

        public AddCommand(ChatController chatController) : base(chatController)
        {
        }

        public async override Task<bool> Execute(ReversoConsole.DbModel.User user, Message message, params string[] param)
        {
            allWords = new AllWordsController();
            learningController = new LearningController(user);
            message.Text = $"Введите слово:";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);       
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
                await chat.ReplyMessage(message);
                return false;
            }

            var newWord = new LearningWord(user, makeWord);
            try
            {
                if (learningController.AddNewWord(newWord))
                {
                    message.Text = $"Добавлено: {newWord} - {newWord.WordToLearn.Translates[0].Text}";
                }
                else
                {
                    message.Text = $"Уже было: {newWord} - {newWord.WordToLearn.Translates[0].Text}";
                }
            }
            catch
            {
                message.Text = $"Ошибка";
            }
            await chat.ReplyMessage(message);
            return false;
        }
    }
}
