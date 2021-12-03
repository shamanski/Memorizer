using System;
using System.Collections.Generic;
using System.Text;
using ReversoConsole.Controller;
using ReversoConsole.DbModel;

namespace ReversoConsole.Algorithm
{
    class StandardLesson : BaseController, ITakingLesson
    {
        private readonly User user;
        public List<LearningWord> Words { get; }
        public StandardLesson(User user)
        {
            this.user = user ?? throw new ArgumentNullException("Пользователь не может быть пустым.", nameof(user));
            Words = GetAllWords();

        }
        private List<LearningWord> GetAllWords()
        {
            return Load<LearningWord>() ?? new List<LearningWord>();
        }
        private void Save()
        {
            Save(Words);

        }
        public Lesson GetNextLesson()
        {
            using (var db = new ReversoConsole.Controller.AppContext())
            {
                throw new NotImplementedException();

            }
        }

        public void ReturnFinishedLesson(Lesson lesson)
        {
            throw new NotImplementedException();
        }
    }
}
