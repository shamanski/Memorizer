﻿using ReversoConsole.Algorithm;
using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversoConsole.Controller
{
    public class LearningController: BaseController
    {
        private User user;
        private List<LearningWord> Words { get { return user.Words; } set { } }
        public ITakingLesson Lesson { get; }
        public LearningController(User user)
        {
            this.user = user ?? throw new ArgumentNullException("Username is empty", nameof(user));
            LoadAll();
            this.Lesson = new StandardLesson(user);
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
