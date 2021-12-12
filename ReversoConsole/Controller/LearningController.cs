using ReversoConsole.Algorithm;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversoConsole.Controller
{
    class LearningController: BaseController
    {
        private readonly User user;
        public List<LearningWord> Words { get; }
        public ITakingLesson Lesson { get; }
        public LearningController(User user)
        {
            this.user = user ?? throw new ArgumentNullException("Username is empty", nameof(user));
            this.Words = user.Words;
            this.Lesson = new StandardLesson(user);
        }

        private void Save()
        {
            Save(Words);
        }

        public LearningWord Find(string name)
        {
            return Words.Where(i => i.WordToLearn.Text == name).FirstOrDefault();
        }

        public void AddNewWord(LearningWord word)
        {
           if (Find(word.WordToLearn.Text) == null)
            {
                Words.Add(word);
                Update(word);
            }

           else
            {
                throw new ArgumentException($"Word '{word.WordToLearn.Text}' is already added");
            }
            
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
                Delete(word);
                return true;
            }

            return false;
        }

        public void UpdateCheckedWords(List<LearningWord> words)
        {
           // Words = words;
        }
    }
}
