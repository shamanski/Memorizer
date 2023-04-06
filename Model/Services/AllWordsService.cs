using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using ReversoApi.Models;
using System.Threading.Tasks;
using ReversoApi;
using ReversoApi.Models.Word;
using Microsoft.EntityFrameworkCore;
using Model.Data.Repositories;

namespace Model.Services
{
    public class AllWordsService : BaseController
    {
        private readonly IGenericRepository<Word> repository;
        public AllWordsService(IGenericRepository<Word> repository)
        {
            this.repository = repository;
        }

        public async Task<Word> FindWordByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            name = string.Concat(name[0].ToString().ToUpper(), name.AsSpan(1));
            if (repository.GetByConditionAsync(f => f.Text == name).Result.Any())
            {
                var words = await repository.GetByConditionAsync(i => i.Text == name);
                 //   .Include(i => i.Translates)
                 return words.First();
            }

            else
            {
                var res = await Do(name);
                if (res == null) return null;
                await repository.AddAsync(res);
                await repository.SaveChangesAsync();
                return res;
            }
        }

        public async Task<List<Word>> FindWordsById(int startId, int count)
        {
            var words = await repository.GetPagedAsync(startId / count, count );
            return words.Items;
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
                             where !translate.IsRude && translate.IsFromDict
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
