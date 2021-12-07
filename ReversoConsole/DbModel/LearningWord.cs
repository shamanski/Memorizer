using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.DbModel
{
    class LearningWord
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Word WordToLearn { get; set; }
        public int Level { get; set; }
        public DateTime? LastTime { get; set; }
        public LearningWord() { }
        public LearningWord(User user, Word wordToLearn)
        {
            this.User = user;
            this.WordToLearn = wordToLearn;
        }

    }
}
