using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ReversoApi.Models;
using System.Threading.Tasks;
using ReversoApi;
using ReversoApi.Models.Word;

namespace ReversoConsole.Controller
{
    class AllWordsController: BaseController
    {
        public List<Word> Words { get; }
        public AllWordsController()
        {
            Words = GetAllWords();
        }
        private List<Word> GetAllWords()
        {
            return Load<Word>() ?? new List<Word>();
        }
        public Word FindWordByName(string name)
        {
            var result = Words.SingleOrDefault(f => f.Text == name);
            if (result == null)
            {
                
                var res =  Do(name).Result ?? null;
                Words.Add(res);
                Update(res);
                return res;
            }
            else
            {
                return result;
            }

            return new Word();
        }
        public void Save()
        {
            Save(Words);
        }
        private static async Task<Word> Do(string wordName)
        {
            var service = new ReversoService();
            TranslatedResponse translatedWord = await service.TranslateWord(new TranslateWordRequest(from: Language.En, to: Language.Ru)
            {
                Word = wordName
            });
            var w = new Word();
            if (!translatedWord.Error && translatedWord.Success)
            {
                w.Text = translatedWord.Sources[0].DisplaySource;
                var items = (from translate in translatedWord.Sources[0].Translations
                             select new Translate
                             {
                                 Text = translate.Translation
                             }).ToList();
                w.TranslatesList = new List<Translate>();
                w.TranslatesList.AddRange(items);
            }      
            return w;
        }

        
    }
}
