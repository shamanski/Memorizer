using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.DbModel
{
    public class LearningWord : LearningModelBase
    {
        public override int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int WordToLearnId { get; set; }
        public virtual Word WordToLearn { get; set; }
        public int Level { get; set; }
        public DateTime LastTime { get; set; }
        public LearningWord() { }
        public LearningWord(User user, Word wordToLearn)
        {
            this.User = user;
            this.WordToLearn = wordToLearn;
            this.LastTime = DateTime.Now;
        }

    }
}
