using System.Collections.Generic;

namespace ReversoConsole.DbModel
{
    class Word
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Phrase> PhrasesList { get; set; }
        public List<Translate> TranslatesList { get; set; }
    }
}
