using Memorizer.DbModel;
using Model.Services;
using ReversoApi.Models;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using User = Memorizer.DbModel.User;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/add - Добавить слово")]
    public class AddCommand : BotCommand, IBotCommand 
    {
        public override string Name { get; } = "/add";
        private readonly LearningService learningService;
        private readonly AllWordsService allWords;

        public AddCommand(ChatController chatController, AllWordsService allWords, LearningService learningService) : base(chatController)
        {
            this.allWords = allWords;
            this.learningService = learningService;
        }

        public async override Task<bool> Execute(User user, Message message, params string[] args)
        {           
            message.Text = $"Введите слово:";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);       
            return true;
        }

        public async override Task<bool> Next(User user, Message message)
        {
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
