using System;
using System.Collections.Generic;
using System.Text;
using ReversoConsole.DbModel;

namespace ReversoConsole.Algorithm
{
    interface IUserInterface
    {
        public List<LearningWord> GetCheckedWords();
        public void UpdateCheckedWords(List<LearningWord> wordList);
        public void AddNewWord(LearningWord word);
        public void RemovecheckedWord(LearningWord word);
    }
}
