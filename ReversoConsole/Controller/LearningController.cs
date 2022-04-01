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
        private List<LearningWord> Words { get { return user.Words; } }
        
        public LearningController(User user)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user), "Username is empty");
            LoadAll();
        }

        private void LoadAll()
        {
            if (Words?.FirstOrDefault()?.WordToLearn == null)
            {
                var result = new List<LearningWord>();
                foreach (var i in Words)
                {
                    var el = LoadElement<LearningWord>(i, nameof(LearningWord.WordToLearn));
                    el.WordToLearn = LoadElement<Word>(el.WordToLearn, nameof(el.WordToLearn.Translates));
                    result.Add(el);
                }
                user.Words = result;
            }
        }

        public List<LearningWord> GetAll()
        {
            return Words;
        }

        public LearningWord Find(string name)
        {
            name = string.Concat(name[0].ToString().ToUpper(), name.AsSpan(1));
            return Words.FirstOrDefault(i => i.ToString() == name);
        }

        public bool AddNewWord(LearningWord word)
        {
           if (Find(word.ToString()) == null)
            {
                Words.Add(word);
                Update(word);
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

        public List<LearningWord> GetCheckedWords()
        {
            return Words;
        }

        public bool RemoveWord(LearningWord word)
        {
            if (user.Words.Remove(word))
            {
               Words.Remove(word);          
               return true;
            }

            return false;
        }
    }
}
