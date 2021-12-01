using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.Algorithm
{
    interface ITakingLesson
    {
        public Lesson GetNextLesson();
        public void ReturnFinishedLesson(Lesson lesson);
    }
}
