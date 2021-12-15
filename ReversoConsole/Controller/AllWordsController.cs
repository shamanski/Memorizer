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
    public class AllWordsController: BaseController
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

        private void LoadWord(ref Word inp)
        {
            inp = LoadElement<Word>(inp, nameof(inp.Translates));
        }

        public Word FindWordByName(string name)
        {
            var result = Words.SingleOrDefault(f => f.Text == name);
            if (result == null)
            {
                var res = Do(name).Result ?? null;
                if (res == null)
                {
                    throw new Exception("Translate server error");
                };
                Words.Add(res);
                Update(res);
                return res;
            }

            else
            {
                var update = this.Words.Find(i => i.Text == name);
                LoadWord(ref update);
                return update;
            }
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
            
            if (!translatedWord.Error && translatedWord.Success)
            {
                Word w = new Word
                {
                    Text = translatedWord.Sources.First().DisplaySource
                };
                var items = (from translate in translatedWord.Sources.First().Translations
                             where (translate.IsRude == false)
                             select new Translate
                             {
                                 Text = translate.Translation
                             }).ToList();
                w.Translates = new List<Translate>();                
                w.Translates.AddRange(items);
                return w;
            }

            return null;
        }

        
    }
}
