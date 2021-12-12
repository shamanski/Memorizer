using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.DbModel
{
    public class User : LearningModelBase
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public virtual List<LearningWord> Words { get; set; }
        public User() { }
        public User(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name is empty or null", nameof(name));
            }

            Name = name;
            Words = new List<LearningWord>();
        }

    }
}
