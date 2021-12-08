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
        public List<LearningWord> Words { get; private set; }
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
            var rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                list.Add(Words.Where(w => w.Id == rnd.Next(1,Words.Count)).Select(u => u.WordToLearn.Text).FirstOrDefault());
            }
            return list;
        }
        private DateTime GetNextTime(LearningWord word)
        {
            return word.LastTime+TimeSpan.FromMinutes(settings.PeriodsInMinutes[word.Level]);
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
            var newWords = Words.Where(i => i.Level == 0).Take(settings.NewWordsInLesson).ToList();
            foreach (var word in newWords)
            {
                lesson.WordsList.Add(MakeLessonWord(word));
            }
            int repeatWords = settings.WordsInLesson - newWords.Count();
            var repeat = Words.OrderBy( i => GetNextTime( i ) ).Take(repeatWords);
            foreach (var word in repeat)
            {
                lesson.WordsList.Add(MakeLessonWord(word));
            }
            return lesson;
        }

        public void ReturnFinishedLesson(Lesson lesson)
        {
            var words = new List<LearningWord>();
            foreach (var i in lesson.WordsList)
            {
                if (i.isSuccessful == IsSuccessful.True)
                {
                    int lvl = (i.Level < settings.maxLevel) ? i.Level++ : i.Level;
                    words.Add(new LearningWord(user, i.Word) { LastTime = DateTime.Now, Level = lvl }) ;
                }
                else if (i.isSuccessful == IsSuccessful.False)
                {
                    int lvl = (i.Level > 0) ? i.Level-- : i.Level;
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
