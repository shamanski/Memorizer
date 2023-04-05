using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Model.Data.Repositories;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Services
{
    public class UserService : IUserService, ICurrentUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private User currentUser;

        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        

        public async Task SetCurrentUser(string id)
        {
            var users = await _userRepository.GetByConditionAsync(i => i.TelegramId == id);
            this.currentUser = await users.FirstOrDefaultAsync();
        }

        public User GetCurrentUser()
        {
            return this.currentUser;
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetUserByTelegramIdAsync(string id)
        {
            var user = await _userRepository.GetByConditionAsync(i => i.TelegramId == id);
            return await user.FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
