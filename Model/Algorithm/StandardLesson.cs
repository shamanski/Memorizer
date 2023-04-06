using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Memorizer.DbModel;
using System.Threading.Tasks;
using Model.Services;
using Model.Data.Repositories;
using Model.Extensions;

namespace Memorizer.Algorithm
{
    public class StandardLesson : BaseController, ILessonService<Lesson>
    {
        public readonly LessonSetings settings; 
        private readonly MyUserService users;
        private readonly IGenericRepository<LearningWord> repository;



        public StandardLesson(MyUserService users, IGenericRepository<LearningWord> repository)
        {
            this.users = users;
            this.repository = repository;
            settings = new LessonSetings();            
        }

        private async Task<List<string>> GetAdditionalWords(string source)
        {
            var user = users.GetCurrentUser();
            var additional = await repository.GetByConditionAsync(x => (x.UserId == user.Id) && (x.WordToLearn.Text != source));
            return additional.OrderBy(x => Guid.NewGuid())
            .Take(5)
            .Select(x => x.WordToLearn.Text)
            .ToList();               
        }

        private DateTime GetNextTime(LearningWord word)
        {
            return DateTime.Now + TimeSpan.FromMinutes(settings.PeriodsInMinutes[word.Level]);
        }

        private async Task<LessonWord> MakeLessonWord(LearningWord word)
        {
            
            return new LessonWord
            {
                LearningWord = word,                  
                AdditionalWords = await GetAdditionalWords(word.ToString())
            };
        }

        public async Task<Lesson> GetNextLesson()
        {
            var user = users.GetCurrentUser();
            if (repository.Count() < settings.NewWordsInLesson) throw new ArgumentException("Nothing to learn");
            var lesson = new Lesson();
            var newWords = await repository.GetByConditionAsync(i => (i.UserId == user.Id) && (i.Level == 0));
             var newWordsList =   newWords.OrderBy(i => i.Id)
                .Take(settings.NewWordsInLesson)
                .Include(x => x.WordToLearn)
                .ThenInclude(x => x.Translates)
                .ToList();
            var repeat = await repository.GetByConditionAsync(i => (i.UserId == user.Id) && (i.Level > 0));             
               var repeatList = repeat.OrderBy(i => i.LastTime)
                .Take(settings.WordsInLesson - newWordsList.Count)
                .Include(x => x.WordToLearn)
                .ThenInclude(x => x.Translates)
                .ToList();
            repeatList.AddRange(newWordsList);
            foreach (var word in repeatList)
            {
                lesson.WordsList.Add(await MakeLessonWord(word));
            }
            return await Task.FromResult(lesson);
        }

        public async Task ReturnFinishedLesson(Lesson lesson)
        {
            
            foreach (var i in lesson.WordsList)
            {
                var entry = await repository.GetByIdAsync(i.LearningWord.Id);
                entry.LastTime = GetNextTime(i.LearningWord);
                entry.Level = i.IsSuccessful switch
                {
                    IsSuccessful.True when (i.LearningWord.Level < settings.maxLevel) => i.LearningWord.Level + 1,
                    IsSuccessful.False => 1,
                    IsSuccessful.Finished => -1,
                    _ => i.LearningWord.Level
                };

                await repository.UpdateAsync(entry); 
            }

            await repository.SaveChangesAsync();

        }
    }
}
