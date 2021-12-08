using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.Algorithm
{
    
    class LessonSetings
    {

        public int[] PeriodsInMinutes { get; } = new int[8] {0, 240, 720, 8640, 17280, 69120, 129600, 259200 };
        public int WordsInLesson { get; } = 10;
        public int NewWordsInLesson { get; } = 2;
        public readonly int maxLevel = 7;
    }
}
