using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole
{
    class Model
    {
        private Dictionary<string, Translate> _words;
        public Model() 
        {
            _words = new Dictionary<string, Translate>();
        }

    }

    class Translate
    {
        public List<string> translates;
        public List<string> phrases;
    }
}
