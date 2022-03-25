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
    class RemoveCommand : BotCommand
    {
        public override string Name { get; } = "Remove word(s) from your list";
        private LearningController learningController;

        public async override Task<bool> Execute(ReversoConsole.DbModel.User user, Message message)
        {
            learningController = new LearningController(user);
            message.Text = $"Введите слово:";
            message.ReplyMarkup = null;
            await ChatController.ReplyMessage(message);
            return true;
        }

        public async override Task<bool> Next(ReversoConsole.DbModel.User user, Message message)
        {
            try
            {
                var word = learningController.Find(message.Text);
                if (learningController.RemoveWord(word) )
                     message.Text = $"Удалено {word}";
                else
                {
                    message.Text = $"Такого слова не найдено {word}";
                }
            }
            catch
            {
                message.Text = $"Ошибка связи с сервером. Попробуйте позже.";
                await ChatController.ReplyMessage(message);
                return false;
            }

            await ChatController.ReplyMessage(message);
            return false;
        }
    }
}
