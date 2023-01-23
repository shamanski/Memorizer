using System;
using System.Collections.Generic;
using System.Text;

namespace Memorizer.Algorithm
{
    
    public class LessonSetings
    {

        public int[] PeriodsInMinutes { get; init; } = new int[8] {0, 240, 720, 8640, 17280, 69120, 129600, 259200 };
        public int WordsInLesson { get; init; } = 10;
        public int NewWordsInLesson { get; init; } = 2;
        public int maxLevel = 7;
    }
}
