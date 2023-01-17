using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using ReversoApi.Models;
using System.Threading.Tasks;
using ReversoApi;
using ReversoApi.Models.Word;
using Microsoft.EntityFrameworkCore;

namespace Memorizer.Controller
{
    public class AllWordsController: BaseController
    {
        private readonly WebAppContext _context;
        public AllWordsController(WebAppContext context)
        {
            this._context = context;
        }

        public List<Word> GetAllWords()
        {
            return _context.Words.ToList();
        }

        public Word FindWordByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;  
            name = string.Concat(name[0].ToString().ToUpper(), name.AsSpan(1));
            if (_context.Words.Where(f => f.Text == name).Any())
            {
                return _context.Words
                    .Where(i => i.Text == name)
                    .Include(i => i.Translates)
                    .First();
            }

            else
            {
                var res = Do(name).Result;
                if (res == null) return null;
                _context.Words.Add(res);
                _context.SaveChanges();
                return res;
            }
        }

        public List<Word> FindWordsById(int start, int count) => 
            _context.Words
                .Skip(start)
                .Take(count)
                .Include(i => i.Translates)
                .ToList();

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
