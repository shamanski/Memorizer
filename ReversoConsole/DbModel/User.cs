using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.DbModel
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LearningWord> Words { get; set; }

    }
}
