using System.Collections.Generic;
using ReversoConsole.DbModel;

namespace ReversoConsole.Algorithm
{
    public interface IUserInterface
    {
        public List<LearningWord> GetCheckedWords();
        public void UpdateCheckedWords(List<LearningWord> wordList);
        public void AddNewWord(LearningWord word);
        public void RemovecheckedWord(LearningWord word);
    }
}
