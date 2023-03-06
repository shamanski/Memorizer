using System.Collections.Generic;

namespace TgBot.BotCommands
{
    public class StateController<T>
    {
        private Dictionary<string, T> UserState { get; set; } 

        public StateController() => UserState = new Dictionary<string, T>();

        public void Add(string user, T cmd) => UserState.TryAdd(user, cmd);

        public T GetUserState(string user)
        {
            T value = default(T);
            UserState?.TryGetValue(user,out value);
            return value;
        }

        public void RemoveUserState(string user) => UserState.Remove(user);

    }
}
