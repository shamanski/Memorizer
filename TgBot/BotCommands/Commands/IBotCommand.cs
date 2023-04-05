using Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using User = Memorizer.DbModel.User;

namespace TgBot.BotCommands.Commands
{
    public interface IBotCommand
    {
        string Name { get; }
        Task<bool> Execute(User user, Message message, string[] args);
    }
}
