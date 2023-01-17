using System;
using System.Collections.Generic;
using System.Text;
using Memorizer.DbModel;

namespace Memorizer.Algorithm
{
    public enum IsSuccessful
    {
        NotStarted,
        False,
        True,
        Finished
    }

    public class LessonWord
    {
        public LearningWord LearningWord { get; set; }
        public IsSuccessful IsSuccessful { get; set; } = IsSuccessful.NotStarted;
        public List<String> AdditionalWords { get; set; }
        public override string ToString() => LearningWord.ToString();
    }
   

}
