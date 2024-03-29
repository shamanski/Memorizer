﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBot.Keybords
{
    class LessonKeyboard
    {
        public InlineKeyboardMarkup Keyboard { get; set; }
        public LessonKeyboard(List<string> words, string sourceWord) 
        {
            if (words == null || words.Count != 6)
            {
                throw new ArgumentException(nameof(words));
            }

            words = words.Select((x) => x.Insert(0, '\u25A1'.ToString())).ToList();
            var keyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton> {InlineKeyboardButton.WithCallbackData(words[0]), InlineKeyboardButton.WithCallbackData(words[1]) },
                    new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(words[2]), InlineKeyboardButton.WithCallbackData(words[3]) },
                    new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData(words[4]), InlineKeyboardButton.WithCallbackData(words[5]) },
                    new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData('\u26a1'.ToString() + "Уже запомнил", "!rm") }
                };

            Keyboard = new InlineKeyboardMarkup(keyboard);

        }      
    }
}
