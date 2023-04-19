using System;
using System.Collections.Generic;

namespace Memorizer.DbModel
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string TelegramId { get; set; }
        public string TelegramName { get; set; }
        public string WebAppId { get; set; }
        public string Email { get; set; }

        public User() { }
        public User(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name is empty or null");
            }

            Name = name;
        }
    }
}
