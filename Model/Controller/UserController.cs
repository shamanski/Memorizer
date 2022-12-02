using ReversoConsole.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ReversoConsole.Controller
{
    public class UserController : BaseController
    {
        private readonly WebAppContext _context;
        public List<User> Users { get => _context.Users.ToList(); }
        public UserController(WebAppContext context)
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
