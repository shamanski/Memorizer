using System.Collections.Generic;

namespace Memorizer.DbModel
{
    public class Word : BaseEntity
    {
        public string Text { get; set; }
        public virtual List<Phrase> PhrasesList { get; set; }
        public virtual List<Translate> Translates { get; set; }
    }
}
