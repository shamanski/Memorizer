using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<string> GetAdditionalWords()
        {
            var list = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(Words.Where(w => w.Id == i).Select(u => u.WordToLearn.Text).FirstOrDefault());
            }
            return list;
        }
        private LessonWord MakeLessonWord(LearningWord word)
        {
            return new LessonWord
            {
                Word = word.WordToLearn,
                TimeFinished = word.LastTime,
                AdditionalWords = GetAdditionalWords()
            };
        }
        public Lesson GetNextLesson()
        {
            var lesson = new Lesson();
            var newWords = Words.Where(i => i.Level == 0).Take(2);
            foreach (var word in newWords)
            {
                lesson.WordsList.Add(MakeLessonWord(word));
            }
            return lesson;
        }

        public void ReturnFinishedLesson(Lesson lesson)
        {
            throw new NotImplementedException();
        }
    }
}
