namespace Memorizer.DbModel
{
    public class Phrase : BaseEntity
    {
        public string PhraseTextSource { get; set; }
        public string PhraseTextTranslate { get; set; }
    }
}
