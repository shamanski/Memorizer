using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBot.Keybords
{
    class LessonKeyboard
    {
        public InlineKeyboardMarkup Keyboard { get; set; }
        public LessonKeyboard() 
        {
            var i = InlineKeyboardButton.WithCallbackData("string");
            var keyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton> {InlineKeyboardButton.WithCallbackData("strinfgdslkjgfsdlkjghfdjkhgfjkdgdfg"), InlineKeyboardButton.WithCallbackData("string") },
                    new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("string"), InlineKeyboardButton.WithCallbackData("string") },
                    new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("string"), InlineKeyboardButton.WithCallbackData("string") }
                   
                   
                };
            keyboard.Add(LessonMenu());

            Keyboard = new InlineKeyboardMarkup(keyboard);

        }

        private List<InlineKeyboardButton> LessonMenu()
        {
            return new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("string"),
                InlineKeyboardButton.WithCallbackData("string"),
                InlineKeyboardButton.WithCallbackData("string")
            };
        }
    }
}
