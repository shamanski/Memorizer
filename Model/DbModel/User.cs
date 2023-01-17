using System;
using System.Collections.Generic;

namespace Memorizer.DbModel
{
    public class User : LearningModelBase
    {
        public override int Id { get; set; }
        public string Name { get; set; }
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
