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
            this.user = user ?? throw new ArgumentNullException("Пользователь не может быть пустым.", nameof(user));
            this.Words = user.Words;
            this.Lesson = new StandardLesson(user);
        }


        private void Save()
        {
            Save(Words);
        }
        public void AddNewWord(LearningWord word)
        {
            Words.Add(word);
            Update(word);
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
           // Words = words;
        }
    }
}
