namespace BlazorFront.Pages
{
    public partial class Lesson
    {
        public bool Completed { get; set; } = false;
        public List<LessonWord> WordsList { get; set; } = new List<LessonWord>();
        public long Id { get; set; } = 0;
    }

    public enum IsSuccessful
    {
        NotStarted,
        False,
        True,
        Finished
    }

    public class LessonWord
    {
        public LearningWord LearningWord { get; set; }
        public IsSuccessful IsSuccessful { get; set; } = IsSuccessful.NotStarted;
        public List<String> AdditionalWords { get; set; }
        public override string ToString() => LearningWord.ToString();
    }

    public class LearningWord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WordToLearnId { get; set; }
        public Word WordToLearn { get; set; }
        public int Level { get; set; }
        public DateTime LastTime { get; set; }
        public LearningWord() { }
        public LearningWord(Word wordToLearn)
        {
            this.WordToLearn = wordToLearn;
            this.LastTime = DateTime.Now;
        }
        public override string ToString() => WordToLearn.Text;
    }

    public class Word
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
