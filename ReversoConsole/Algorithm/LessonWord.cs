using System;
using System.Collections.Generic;
using System.Text;
using ReversoConsole.DbModel;

namespace ReversoConsole.Algorithm
{
    public enum IsSuccessful
    {
        NotStarted,
        False,
        True
    }

    class LessonWord
    {
        public LearningWord LearningWord { get; set; }
        public IsSuccessful isSuccessful { get; set; } = IsSuccessful.NotStarted;
        public List<String> AdditionalWords { get; set; }

    }
}
