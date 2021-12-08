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
        public readonly LessonSetings settings;
        public List<LearningWord> Words { get; }
        public StandardLesson(User user)
        {
            this.user = user ?? throw new ArgumentNullException("Username is null or empty", nameof(user));
            Words = user.Words;
            settings = new LessonSetings();
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
        private DateTime GetNextTime(LearningWord word)
        {
            return word.LastTime+word.Level;
        }
        private LessonWord MakeLessonWord(LearningWord word)
        {
            return new LessonWord
            {
                Word = word.WordToLearn,
                Level = word.Level,
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
            //var repeat = Words.GroupBy(i => i.Level).
            return lesson;
        }

        public void ReturnFinishedLesson(Lesson lesson)
        {
            var words = new List<LearningWord>();
            foreach (var i in lesson.WordsList)
            {
                if (i.isSuccessful == IsSuccessful.True)
                {
                    int lvl = (i.Level < 7) ? i.Level++ : i.Level;///TODO: Remove magic number
                    words.Add(new LearningWord(user, i.Word) { LastTime = DateTime.Now, Level = lvl }) ;
                }
                else if (i.isSuccessful == IsSuccessful.False)
                {
                    int lvl = (i.Level > 0) ? i.Level-- : i.Level;///TODO: Remove magic number
                    words.Add(new LearningWord(user, i.Word) { LastTime = DateTime.Now, Level = lvl });
                }
                else
                {
                    words.Add(new LearningWord(user, i.Word) { LastTime = i.TimeFinished, Level = i.Level });
                }
            }
        }
    }
}
