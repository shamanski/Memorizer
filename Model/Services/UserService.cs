using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Model.Data.Repositories;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Model.Services
{
    public class MyUserService : IUserService, ICurrentUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<TelegramCode> _codes;
        private User currentUser;

        public MyUserService(IGenericRepository<User> userRepository, IGenericRepository<TelegramCode> codes)
        {
            _userRepository = userRepository;
            _codes = codes;
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

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByConditionAsync(i => i.Email == email).Result.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByTelegramIdAsync(string id)
        {
            var user = await _userRepository.GetByConditionAsync(i => i.TelegramId == id);
            var ret = await user.FirstOrDefaultAsync();
            if (ret == null)
            {
                throw new ArgumentNullException("User doesn't exists");
            }
            return ret;
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
            await _userRepository.SaveChangesAsync();
        }

        public async Task LinkTelegramAccountAsync(User user, string telegramUserId, string VerifyCode)
        {            


            if (user.TelegramId != null)
            {
                throw new InvalidOperationException("User already has a linked Telegram account");
            }

            var existingUser = await _userRepository.GetByConditionAsync(i => i.TelegramId == telegramUserId);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Telegram account is already linked to another user");
            }

            var c = await _codes.GetByConditionAsync(i => i.Code == VerifyCode);
            var code = await c.FirstOrDefaultAsync();
            if (code == null)
            {
                throw new InvalidOperationException("Invalid code");
            }

            user.TelegramId = telegramUserId;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
