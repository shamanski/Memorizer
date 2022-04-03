using Microsoft.EntityFrameworkCore;
using ReversoConsole.Algorithm;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversoConsole.Controller
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

        public LearningWord Find(string name)
        {
            name = string.Concat(name[0].ToString().ToUpper(), name.AsSpan(1));
            return _context.LearningWords.Where(i => i.UserId == user.Id).FirstOrDefault(i => i.ToString() == name);
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
            int count = 0;
            foreach (var word in words)
            {
                if (AddNewWord(new LearningWord(user, word))) count++;
            }
            return count;
        }

        public bool RemoveWord(LearningWord word)
        {
            _context.LearningWords.Remove(word);
            _context.SaveChangesAsync();

            return true;
        }
    }
}
