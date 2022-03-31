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

        public AddWordsKeyboard()
        {
            var keyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> {new InlineKeyboardButton("Слова 1..100") { CallbackData = "/startpack 1 100" }, new InlineKeyboardButton("Слова 101..200") { CallbackData = "/startpack 101 100" } },
                
                new List<InlineKeyboardButton> {new InlineKeyboardButton("Слова 201..300") { CallbackData = "/startpack 201 100" }, new InlineKeyboardButton("Слова 301..400") { CallbackData = "/startpack 301 100" }  },
                
                new List<InlineKeyboardButton> {new InlineKeyboardButton("Слова 401..500") { CallbackData = "/startpack 401 100" }, new InlineKeyboardButton("Слова 501..600") { CallbackData = "/startpack 501 100" } },
                
                new List<InlineKeyboardButton> {new InlineKeyboardButton("Слова 601..700") { CallbackData = "/startpack 601 100" }, new InlineKeyboardButton("Слова 701..800") { CallbackData = "/startpack 701 100" } }
                
            };           

            Keyboard = new InlineKeyboardMarkup(keyboard);

        }                 
    }
}
