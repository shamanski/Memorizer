using ReversoConsole.DbModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBot.BotCommands
{
    public abstract class BotCommand
    {
        public abstract string Name { get; }

        public abstract Task Execute(ReversoConsole.DbModel.User user, Message arguments);


        public abstract bool Next(ReversoConsole.DbModel.User user, Message arguments);
    }
}
