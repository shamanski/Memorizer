using System;
using System.Collections.Generic;
using System.Text;
using ReversoConsole.DbModel;

namespace ReversoConsole.Algorithm
{
    class Lesson
    {
        public bool Completed { get; set; } = false;
        public List<LessonWord> WordsList { get; set; }
    }
}
