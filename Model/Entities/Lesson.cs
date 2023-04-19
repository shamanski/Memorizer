using System;
using System.Collections.Generic;
using System.Text;
using Memorizer.Algorithm;
using Memorizer.DbModel;

namespace Memorizer.DbModel
{
    public class Lesson : BaseEntity
    {
        public bool Completed { get; set; } = false;
        public List<LessonWord> WordsList { get; set; } = new List<LessonWord>();
        public int UserId { get; set; }
    }
}
