using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBot.Keybords
{ 
        public class PaginationService
        {
            private readonly TelegramBotClient _telegramBotClient;
            private readonly List<string> _items;
            private readonly int _pageSize;

            public PaginationService(TelegramBotClient telegramBotClient, List<string> items, int pageSize)
            {
                _telegramBotClient = telegramBotClient;
                _items = items;
                _pageSize = pageSize;
            }

            public async void SendPaginatedMessageAsync(ChatId chatId)
            {
                int currentPage = 0;

                while (true)
                {
                    var itemsOnCurrentPage = _items
                        .Skip(currentPage * _pageSize)
                        .Take(_pageSize)
                        .ToList();

                    var message = string.Join("\n", itemsOnCurrentPage);
                    var replyMarkup = GetReplyMarkup(currentPage);

                    var sentMessage = await _telegramBotClient.SendTextMessageAsync(
                        chatId,
                        message,
                        replyMarkup: replyMarkup
                    );

                var userResponse = await _telegramBotClient.AnswerCallbackQueryAsync(" ");

                    if (userResponse.Data == "previous")
                    {
                        currentPage--;
                    }
                    else if (userResponse.Data == "next")
                    {
                        currentPage++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            public InlineKeyboardMarkup GetReplyMarkup(int currentPage)
            {
                var buttons = new List<InlineKeyboardButton>();

                if (currentPage > 0)
                {
                    buttons.Add(InlineKeyboardButton.WithCallbackData("Previous", "previous"));
                }

                if (currentPage < (_items.Count - 1) / _pageSize)
                {
                    buttons.Add(InlineKeyboardButton.WithCallbackData("Next", "next"));
                }

                return new InlineKeyboardMarkup(new[] { buttons });
            }
        }
    }

