using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace ReversoApi.Models
{

    [Serializable]
    public class Context
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public bool IsGood { get; set; }
    }

    [Serializable]
    public class Translations
    {
        public string Translation { get; set; }
        public int Count { get; set; }
        public List<Context> Contexts { get; set; }
        public bool IsFromDict { get; set; }
        public string Pos { get; set; }
        public bool IsRude { get; set; }
        public bool IsSlang { get; set; }
        public bool IsReverseValidated { get; set; }
        public bool IsGrayed { get; set; }
        public object FavoriteId { get; set; }
    }
    
    [Serializable]
    public class Sources
    {
        public int Count { get; set; }
        public string Source { get; set; }
        public string DisplaySource { get; set; }
        public List<Translations> Translations { get; set; }
        public bool SpellCorrected { get; set; }
        public string DirectionFrom { get; set; }
        public string DirectionTo { get; set; }
    }

    [Serializable]
    public class TranslatedResponse: ResporseError
    {
        public List<Sources> Sources { get; set; }
        public string WordSentence { get; set; }
    }
  
}
