using Memorizer.DbModel;
using Model.Services;
using ReversoApi.Models;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using User = Memorizer.DbModel.User;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/add - Добавить слово")]
    public class AddCommand : BotCommand
    {
        public override string Name { get; } = "/add";
        private LearningService learningService;
        private AllWordsService allWords;

        public AddCommand(ChatController chatController) : base(chatController)
        {
        }

        public async override Task<bool> Execute(User user, WebAppContext context, Message message, params string[] param)
        {           
            message.Text = $"Введите слово:";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);       
            return true;
        }

        public async override Task<bool> Next(User user, WebAppContext context, Message message)
        {
            allWords = new AllWordsService(context);
            learningService = new LearningService(user, context);
            Word makeWord;

                makeWord = await allWords.FindWordByName(message.Text);               
            if (makeWord == null)
            {
                message.Text = $"Не удалось добавить";
                await chat.ReplyMessage(message);
                return false;
            }
                
            var newWord = new LearningWord(user, makeWord);
            try
            {
                if (learningService.AddNewWord(newWord))
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
                message.Text = $"Ошибка при добавлении";
            }
            await chat.ReplyMessage(message);
            return false;
        }
    }
}
