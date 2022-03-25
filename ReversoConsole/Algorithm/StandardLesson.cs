using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReversoConsole.Controller;
using ReversoConsole.DbModel;

namespace ReversoConsole.Algorithm
{
    public class StandardLesson : BaseController, ITakingLesson
    {
        public readonly LessonSetings settings;       
        public List<LearningWord> Words { get; private set; }
       
        public StandardLesson(User user)
        {
            Words = user.Words;
            settings = new LessonSetings();            
        }

        private void Save()
        {
            Save(Words);
        }

        private List<string> GetAdditionalWords(string source)
        {
           var rnd = new Random();
           return Words.Where(x => x.ToString() != source).
                OrderBy(x => rnd.Next()).
                Take(5).
                Select(x => x.
                ToString()).
                ToList();
        }

        private DateTime GetNextTime(LearningWord word)
        {
            return word.LastTime+TimeSpan.FromMinutes(settings.PeriodsInMinutes[word.Level]);
        }

        private LessonWord MakeLessonWord(LearningWord word)
        {
            return new LessonWord
            {
                LearningWord = word,
                AdditionalWords = GetAdditionalWords(word.ToString())
            };
        }

        public Lesson GetNextLesson()
        {
            if (!(Words?.Any() ?? false)) throw new ArgumentException("Nothing to learn");
            var lesson = new Lesson();
            var newWords = Words
                .Where(i => i.Level == 0)
                .Take(settings.NewWordsInLesson);
            var repeat = Words
                .Where(i => i.Level > 0)
                .OrderBy(i => GetNextTime(i))
                .Take(settings.WordsInLesson - newWords.Count())
                .Union(newWords);
                
            foreach (var word in repeat)
            {
                lesson.WordsList.Add(MakeLessonWord(word));
            }
            return lesson;
        }

        public void ReturnFinishedLesson(Lesson lesson)
        {
            foreach (var i in lesson.WordsList)
            {
                i.LearningWord.LastTime = DateTime.Now;
                i.LearningWord.Level = i.IsSuccessful switch
                {
                    IsSuccessful.True when (i.LearningWord.Level < settings.maxLevel) => i.LearningWord.Level + 1,
                    IsSuccessful.False => 1,
                    IsSuccessful.Finished => -1,
                    _ => i.LearningWord.Level
                };
                
                Save();
            }
        }
    }
}
