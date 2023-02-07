using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Model.Services
{
    public class UserService : BaseController
    {
        private readonly WebAppContext _context;
        public List<User> Users { get => _context.Users.ToList(); }
        public UserService(WebAppContext context)
        {
            _context = context;
        }

        public User GetUser(string userName)
        {
            var _user = _context.Users.SingleOrDefault(u => u.Name == userName);
            if (_user == null)
            {
                _context.Users.Add(new User(userName));
                _context.SaveChanges();
                _user = Users.SingleOrDefault(u => u.Name == userName);
            }

            return _user;
        }

    }
}
