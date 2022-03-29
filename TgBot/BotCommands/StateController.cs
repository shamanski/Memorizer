using System.Collections.Generic;

namespace TgBot.BotCommands
{
    public class StateController
    {
        private Dictionary<string, BotCommand> UserState { get; set; } 

        public StateController() => UserState = new Dictionary<string, BotCommand>();

        public void Add(string user, BotCommand cmd) => UserState.Add(user, cmd);

        public BotCommand GetUserState(string user)
        {
            BotCommand value = null;
            UserState?.TryGetValue(user,out value);
            return value;
        }

        public void RemoveUserState(string user) => UserState.Remove(user);

    }
}
