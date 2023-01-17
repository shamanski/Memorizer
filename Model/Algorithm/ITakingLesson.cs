namespace Memorizer.Algorithm
{
    public interface ITakingLesson
    {
        public Lesson GetNextLesson();
        public void ReturnFinishedLesson(Lesson lesson);
    }
}
