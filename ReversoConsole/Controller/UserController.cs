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
        public bool IsNewUser { get; } = false;
        private List<User> GetUsersData()
        {
            var db = new AppContext();
            return db.Users
                .ToList();

        }
 
        public UserController(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("User Name is empty", nameof(userName));
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
            //LoadElement<Word>(ref _user.Words, nameof(LearningWord.WordToLearn));
        }
        public void Save()
        {
            Update(CurrentUser);
        }

        public void SetNewUserData(string Name)
        {
            Save();
        }
    }
}
