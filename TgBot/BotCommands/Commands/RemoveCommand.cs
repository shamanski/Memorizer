﻿using Model.Services;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/remove - Удалить слово")]
    public class RemoveCommand : BotCommand, IBotCommand
    {
        public override string Name { get; } = "/remove";
        private readonly LearningService learning;

        public RemoveCommand(ChatController chatController, LearningService learning) : base(chatController)
        {
            this.learning = learning;
        }

        public async override Task<bool> Execute(Memorizer.DbModel.User user, Message message, params string[] param)
        {
            message.Text = $"Введите слово:";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);
            return true;
        }

        public async override Task<bool> Next(Memorizer.DbModel.User user, Message message)
        {
            try
            {
                var word = await learning.Find(message.Text);
                if (learning.RemoveWord(word) )
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
