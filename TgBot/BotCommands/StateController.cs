using System;
using System.Collections.Generic;
using System.Text;

namespace TgBot.BotCommands
{
    internal class StateController
    {
        private Dictionary<string, BotCommand> userState; 

        public void Add(string user, BotCommand cmd)
        {
            userState ??= new Dictionary<string, BotCommand>();
            userState.Add(user, cmd);
        }
        public BotCommand GetUserState(string user)
        {
            BotCommand value = null;
            userState?.TryGetValue(user,out value);
            return value;
        }

        public void RemoveUserState(string user)
        {
            userState.Remove(user);
        }
    }
}
