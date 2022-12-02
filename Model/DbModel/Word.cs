using System.Collections.Generic;

namespace ReversoConsole.DbModel
{
    public class Word : LearningModelBase
    {
        public override int Id { get; set; }
        public string Text { get; set; }
        public virtual List<Phrase> PhrasesList { get; set; }
        public virtual List<Translate> Translates { get; set; }
    }
}
