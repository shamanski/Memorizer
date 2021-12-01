using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.Algorithm
{
    class UserControl : IUserInterface
    {
        public User CurrentUser { get; set; }
        public void AddNewWord(LearningWord word)
        {
            CurrentUser.Words.Add(word);
        }

        public List<LearningWord> GetCheckedWords()
        {
            return CurrentUser.Words;
        }

        public void RemovecheckedWord(LearningWord word)
        {
            throw new NotImplementedException();
        }

        public void UpdateCheckedWords(List<LearningWord> words)
        {
            CurrentUser.Words = words;
        }
    }
}
