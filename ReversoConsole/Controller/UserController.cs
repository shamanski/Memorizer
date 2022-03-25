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
        private User _user;
        public List<User> Users { get; }
        public User CurrentUser { get { return _user; }  }
        public List<User> InLesson { get; set; }
        public bool IsNewUser { get; } = false;
        private List<User> GetUsersData()
        {
            var db = new AppContext();
            return db.Users
                .ToList();

        }
        public UserController()
        {
            Users = GetUsersData();
        }

        public UserController(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName), "User Name is empty");
            }

            Users = GetUsersData();
            _user = Users.SingleOrDefault(u => u.Name == userName);
            if (_user == null)
            {
                Users.Add(new User(userName));
                IsNewUser = true;
                _user = Users.SingleOrDefault(u => u.Name == userName);
                
            }
            _user = LoadElement<User>(_user, nameof(_user.Words));
        }

        public User GetUser(string userName)
        {
            _user = Users.SingleOrDefault(u => u.Name == userName);
            if (_user == null)
            {
                Users.Add(new User(userName));
                _user = Users.SingleOrDefault(u => u.Name == userName);
                Save();
            }
            _user = LoadElement<User>(_user, nameof(_user.Words));
            return _user;
        }

        public void Save()
        {
            Update(CurrentUser);
        }

    }
}
