using Microsoft.EntityFrameworkCore;
using Memorizer.Algorithm;
using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Model.Extensions;
using Model.Data.Repositories;
using System.Threading.Tasks;

namespace Model.Services
{
    public class LearningService : BaseController
    {
        private readonly MyUserService users;
        private readonly IGenericRepository<LearningWord> repository;


        public LearningService(MyUserService users, IGenericRepository<LearningWord> repository)
        {
             this.users = users;          
             this.repository = repository;
        }     

        public async Task<LearningWord> Find(string text)
        {
            var user = users.GetCurrentUser();
            text = string.Concat(text[0].ToString().ToUpper(), text.AsSpan(1));
            var word = await repository.GetByConditionAsync(i => i.User.Id == user.Id);
              return  word.DefaultIfEmpty()
                .FirstOrDefault(i => i.WordToLearn.Text == text);
        }

        public bool AddNewWord(LearningWord word)
        {
            if (Find(word.ToString()) == null)
            {
                repository.AddAsync(word);
                repository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public int AddNewWords(List<Word> words)
        {
            var user = users.GetCurrentUser();
            var lwords = words
            .Where(x => Find(x.Text) == null)
            .Select(x => new LearningWord(user, x))
            .ToList();
            repository.AddRangeAsync(lwords);
            repository.SaveChangesAsync();
            return lwords.Count;
        }

        public bool RemoveWord(LearningWord word)
        {
            repository.DeleteAsync(word.Id);
            repository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<LearningWord>> GetLearnedWords()
        {
            var user = users.GetCurrentUser();
            var learned = await repository.GetByConditionAsync(x => x.UserId == user.Id && x.Level == -1);
              return learned;
        }

        public int Count()
        {
            return repository.Count();
        }
    }
}
