using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Model.Data.Repositories;
using Newtonsoft.Json;
using ReversoApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TgBot.BotCommands
{
    public class StateController<T>
    {
        private Dictionary<int, T> UserState { get; set; }

        private readonly IGenericRepository<WebAppState> repository;

        public StateController(IGenericRepository<WebAppState> repository)
        {
            UserState = new Dictionary<int, T>();
            this.repository = repository;
        }

        public async Task Add(int userId, T state)
        {
            var userState = await repository.GetByIdAsync(userId);
            if (userState != null)
            {
                userState.StateData = JsonConvert.SerializeObject(state);
            }

            else
            {
                await repository.AddAsync(new WebAppState
                {
                    UserId = userId,
                    StateData = JsonConvert.SerializeObject(state)
                });
            }

            await repository.SaveChangesAsync();
        }

        public async Task<T> GetUserState(User user)
        {
            var state = await repository.GetByIdAsync(user.Id);
       

            if (state != null)
            {
                var d = JsonConvert.DeserializeObject<T>(state.StateData);
                return d;
            }

            else return default(T);
        }

        public async Task RemoveUserState(int userId)
        {
            var userState = await repository.GetByIdAsync(userId);
            if (userState != null)
            {
                await repository.DeleteAsync(userId);
                await repository.SaveChangesAsync();
            }
        }

    }
}
