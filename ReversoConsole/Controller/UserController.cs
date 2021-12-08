using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ReversoConsole.Controller
{
    class UserController : BaseController
    {
        public List<User> Users { get; }
        public User CurrentUser { get; }
        public bool IsNewUser { get; } = false;
        private List<User> GetUsersData()
        {
            return Load<User>() ?? new List<User>();
        }
        public UserController(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("User Name is empty", nameof(userName));
            }

            Users = GetUsersData();

            CurrentUser = Users.SingleOrDefault(u => u.Name == userName);

            if (CurrentUser == null)
            {
                Users.Add(new User(userName));
                IsNewUser = true;
                CurrentUser = Users.SingleOrDefault(u => u.Name == userName);
            }
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
