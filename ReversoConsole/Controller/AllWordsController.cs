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

        private Word LoadWord( Word inp)
        {
            return LoadElement<Word>(inp, nameof(inp.Translates));
        }

        public Word FindWordByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;  
            name = string.Concat(name[0].ToString().ToUpper(), name.AsSpan(1));
            if (!Words.Exists(f => f.Text == name))
            {
                var res = Do(name).Result;
                if (res == null) return null;    
                Words.Add(res);
                Update(res);
                return res;
            }

            else
            {
                var update = this.Words.Find(i => i.Text == name);                
                return LoadWord(update);
            }
        }

        public List<Word> FindWordsById(int start, int count) => 
            Words
                .Skip(start)
                .Take(count)
                .Select((x) => LoadWord(x))
                .ToList();


        public void Save() => Save(Words);


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
                             where (!translate.IsRude && translate.IsFromDict)
                             select new Translate
                             {
                                 Text = translate.Translation
                             }).ToList();
                w.Translates = new List<Translate>();                
                w.Translates.AddRange(items);
                if (w.Translates.Count > 0) return w;
                else w.Translates.AddRange(translatedWord.Sources.First().Translations.Where(i => !i.IsRude && !i.IsGrayed).Select(i => new Translate { Text = i.Translation }));
                return w;
            }

            return null;
        }

        
    }
}
