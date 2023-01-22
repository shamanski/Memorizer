using Microsoft.EntityFrameworkCore;
using Memorizer.Algorithm;
using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Memorizer.Controller
{
    public class LearningController: BaseController
    {
        private readonly User user;
        private readonly WebAppContext _context;

        
        public LearningController(User user, WebAppContext context)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user), "Username is empty");
            _context = context;
        }

        public List<LearningWord> GetAll()
        {
            return _context.LearningWords
                .Where(i => i.UserId == user.Id)
                .Include(i => i.WordToLearn)
                .ThenInclude(i => i.Translates)
                .ToList();
        }

        public LearningWord Find(string text)
        {
            text = string.Concat(text[0].ToString().ToUpper(), text.AsSpan(1));
            return _context.LearningWords
                .Where(i => i.UserId == user.Id)
                .DefaultIfEmpty()
                .FirstOrDefault(i => i.WordToLearn.Text == text);
        }

        public bool AddNewWord(LearningWord word)
        {
           if (Find(word.ToString()) == null)
            {
                _context.LearningWords.Add(word);
                _context.SaveChangesAsync();
                return true; 
            }
            return false;
        }

        public int AddNewWords(List<Word> words)
        {         
                var lwords = words
                .Where(x => Find(x.Text) == null)
                .Select(x => new LearningWord(user, x))
                .ToList();
            _context.LearningWords.AddRange(lwords);
            _context.SaveChangesAsync();
            return lwords.Count;
        }

        public bool RemoveWord(LearningWord word)
        {
            _context.LearningWords.Remove(word);
            _context.SaveChangesAsync();

            return true;
        }
    }
}
