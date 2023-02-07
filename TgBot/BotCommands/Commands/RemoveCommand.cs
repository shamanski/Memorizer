using Model.Services;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/remove - Удалить слово")]
    public class RemoveCommand : BotCommand
    {
        public override string Name { get; } = "/remove";
        private LearningService learningService;

        public RemoveCommand(ChatController chatController) : base(chatController)
        {
        }

        public async override Task<bool> Execute(Memorizer.DbModel.User user, WebAppContext context, Message message, params string[] param)
        {
            learningService = new LearningService(user, context);
            message.Text = $"Введите слово:";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);
            return true;
        }

        public async override Task<bool> Next(Memorizer.DbModel.User user, WebAppContext context, Message message)
        {
            try
            {
                var word = learningService.Find(message.Text);
                if (learningService.RemoveWord(word) )
                     message.Text = $"Удалено {word}";
                else
                {
                    message.Text = $"Такого слова не найдено {word}";
                }
            }
            catch
            {
                message.Text = $"Ошибка связи с сервером. Попробуйте позже.";
                await chat.ReplyMessage(message);
                return false;
            }

            await chat.ReplyMessage(message);
            return false;
        }
    }
}
