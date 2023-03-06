using System;
using System.Collections.Generic;
using System.Text;
using Memorizer.DbModel;

namespace Memorizer.Algorithm
{
    public class Lesson
    {
        public bool Completed { get; set; } = false;
        public List<LessonWord> WordsList { get; set; } = new List<LessonWord>();
        public long Id { get; set; } = 0;
    }
}
