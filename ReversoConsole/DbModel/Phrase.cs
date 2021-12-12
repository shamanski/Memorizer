namespace ReversoConsole.DbModel
{
    public class Phrase : LearningModelBase
    {
        public override int Id { get; set; }
        public string PhraseTextSource { get; set; }
        public string PhraseTextTranslate { get; set; }
    }
}
