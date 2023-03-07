using System;

namespace Memorizer.DbModel
{
    public class LearningWord : BaseEntity
    {
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

        public override string ToString() => WordToLearn.Text;

    }
    
}
