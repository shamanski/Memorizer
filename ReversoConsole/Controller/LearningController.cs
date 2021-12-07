using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.Controller
{
    class LearningController: BaseController
    {
        private readonly User user;
        public List<LearningWord> Words { get; }
        public LearningController(User user)
        {
            this.user = user ?? throw new ArgumentNullException("Пользователь не может быть пустым.", nameof(user));
            Words = GetAllWords();
        }
        private List<LearningWord> GetAllWords()
        {
            return Load<LearningWord>() ?? new List<LearningWord>();
        }

        private void Save()
        {
            Save(Words);
        }
        public void AddNewWord(LearningWord word)
        {
            Words.Add(word);
        }

        public List<LearningWord> GetCheckedWords()
        {
            return Words;
        }

        public void RemovecheckedWord(LearningWord word)
        {
            throw new NotImplementedException();
        }

        public void UpdateCheckedWords(List<LearningWord> words)
        {
            Words = words;
        }
    }
}
