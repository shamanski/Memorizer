using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.Algorithm
{
    class StandardLesson : ITakingLesson
    {
        public Lesson GetNextLesson()
        {
            throw new NotImplementedException();
        }

        public void ReturnFinishedLesson(Lesson lesson)
        {
            throw new NotImplementedException();
        }
    }
}
