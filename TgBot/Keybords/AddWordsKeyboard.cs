using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBot.Keybords
{
    class AddWordsKeyboard
    {
        public InlineKeyboardMarkup Keyboard { get; set; }

        public AddWordsKeyboard(List<string> words)
        {
            var keyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>()
            };
            foreach (var i in words)
            {
                keyboard[0].Add(new InlineKeyboardButton(i) {CallbackData = i });
            }
            keyboard.Add(BottomMenu());

            Keyboard = new InlineKeyboardMarkup(keyboard);

        }

        private List<InlineKeyboardButton> BottomMenu()
        {
            return new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("<<<"),
                InlineKeyboardButton.WithCallbackData("Выбрать все"),
                InlineKeyboardButton.WithCallbackData("Закончить"),
                InlineKeyboardButton.WithCallbackData(">>>")
            };
        }                  
    }
}
