using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Memorizer.Controller;
using Memorizer.DbModel;
using System.Threading.Tasks;

namespace Memorizer.Algorithm
{
    public class StandardLesson : BaseController, ITakingLesson
    {
        public readonly LessonSetings settings; 
        private readonly WebAppContext _context;
        private readonly User user;
        private readonly List<LearningWord> words;

       
        public StandardLesson(User user, WebAppContext context)
        {
            this.user = user;
            _context = context;
            words = _context.LearningWords
                .Where(i => i.UserId == user.Id)
                .Include(i => i.WordToLearn)
                .ThenInclude(i => i.Translates)
                .ToList();
            settings = new LessonSetings();            
        }

        private List<string> GetAdditionalWords(string source)
        {
           var rnd = new Random();
            return words
            .Where(x => x.ToString() != source)
            .OrderBy(x => rnd.Next())
            .Take(5)
            .Select(x => x.ToString())
            .ToList();               
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

        public Lesson GetNextLesson(WebAppContext _context)
        {
            if (!( _context.LearningWords.Where(i => i.UserId == user.Id)?.Any() ?? false)) throw new ArgumentException("Nothing to learn");
            var lesson = new Lesson();
            var newWords = words
                .Where(i => i.Level == 0 )
                .Take(settings.NewWordsInLesson)
                .ToList();
            var repeat = words
                .Where(i => i.Level > 0)
                .OrderBy(i => GetNextTime(i))
                .Take(settings.WordsInLesson - newWords.Count)
                .Union(newWords)
                .ToList();
            foreach (var word in repeat)
            {
                lesson.WordsList.Add(MakeLessonWord(word));
            }
            return lesson;
        }

        public async Task ReturnFinishedLesson(Lesson lesson, WebAppContext context)
        {
            
            foreach (var i in lesson.WordsList)
            {
                var entry = context.LearningWords.Where(x => x.WordToLearnId == i.LearningWord.WordToLearnId).FirstOrDefault();
                entry.LastTime = DateTime.Now;
                entry.Level = i.IsSuccessful switch
                {
                    IsSuccessful.True when (i.LearningWord.Level < settings.maxLevel) => i.LearningWord.Level + 1,
                    IsSuccessful.False => 1,
                    IsSuccessful.Finished => -1,
                    _ => i.LearningWord.Level
                };

                context.Update(entry);  
            }

            await context.SaveChangesAsync();
        }
    }
}
