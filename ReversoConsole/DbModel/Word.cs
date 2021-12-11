using System.Collections.Generic;

namespace ReversoConsole.DbModel
{
    public class Word
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual List<Phrase> PhrasesList { get; set; }
        public virtual List<Translate> Translates { get; set; }
    }
}
