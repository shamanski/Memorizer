using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Model.Data.Repositories;
using System.Text.Json;
using ReversoApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.BotCommands.Commands;
using System.IO;
using System.Text;

namespace TgBot.BotCommands
{
    public class StateController<T>
    {

        private readonly IGenericRepository<WebAppState> repository;

        public StateController(IGenericRepository<WebAppState> repository)
        {
            this.repository = repository;
        }

        public async Task Add(int userId, BotCommand state)
        {
            var userState = await repository.GetByIdAsync(userId);
            if (userState != null)
            {
                userState.StateData = JsonSerializer.Serialize<BotCommand>(state);
            }

            else
            {
                await repository.AddAsync(new WebAppState
                {
                    UserId = userId,
                    StateData = JsonSerializer.Serialize<BotCommand>(state)
                });
            }

            await repository.SaveChangesAsync();
        }

        public async Task<BotCommand> GetUserState(Memorizer.DbModel.User user)
        {
            var state = await repository.GetByConditionAsync(i => i.UserId == user.Id).Result.FirstOrDefaultAsync();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(state.StateData));

            if (state != null)
            {
                var d = JsonSerializer.DeserializeAsync<BotCommand>(stream).Result;
                return d;
            }

            else return default(BotCommand);
        }

        public async Task RemoveUserState(int userId)
        {
            var userState = await repository.GetByConditionAsync(i => i.UserId == userId);
            if (userState != null)
            {
               foreach (var item in userState) await repository.DeleteAsync(item.Id);
               await repository.SaveChangesAsync();
            }
        }

    }
}
