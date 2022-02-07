using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBot.Keybords
{
    class LessonKeyboard
    {
        public InlineKeyboardMarkup Keyboard { get; set; }
        public LessonKeyboard(string[] words) 
        {
            var keyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton> {InlineKeyboardButton.WithCallbackData(words[0]), InlineKeyboardButton.WithCallbackData(words[1]) },
                    new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(words[2]), InlineKeyboardButton.WithCallbackData(words[3]) },
                    new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(words[4]), InlineKeyboardButton.WithCallbackData(words[5]) }
                   
                   
                };

            Keyboard = new InlineKeyboardMarkup(keyboard);

        }      
    }
}
