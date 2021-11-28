using System;
using System.Threading.Tasks;
using ReversoApi;
using ReversoApi.Models;
using ReversoApi.Models.Segment;
using ReversoApi.Models.Text;
using ReversoApi.Models.Word;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ReversoConsole.DbModel;
using ReversoConsole.Controller;
using Microsoft.EntityFrameworkCore;

namespace ReversoApi
{
    [Serializable]
    public class WordDescription
    {
        public string ID  = "0";
        public string Word  = "-";
        public WordDescription(string id, string word)
        {
            this.ID = id;
            this.Word = word;
        }

        public WordDescription() : this("0", "0")
        {
            
        }
    }
    [Serializable]
    public class Words
    {
        public List<WordDescription> words {get;}

        public Words() 
        {
            this.words = new List<WordDescription>();
        }

        public Words(List<WordDescription> words)
        {
            this.words = words;
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {

            using (var db = new ReversoConsole.Controller.AppContext())
            {
                // создаем два объекта User
                var t1 = new List<Translate> { new Translate { Text = "test" } };
                var p1 = new List<Phrase> { new Phrase {PhraseText = "test phrase" } };
                Word w1 = new Word { Text = "word 1", PhrasesList=p1, TranslatesList = t1 };
                Word w2 = new Word { Text = "word 1", PhrasesList = p1, TranslatesList = t1 };

                // добавляем их в бд
                db.Words.Add(w1);
                db.Words.Add(w2);
                db.SaveChanges();
                Console.WriteLine("Объекты успешно сохранены");

                // получаем объекты из бд и выводим на консоль
                var words = db.Words.ToList();
                Console.WriteLine("Список объектов:");
                foreach (Word w in words)
                {
                    Console.WriteLine($"{w.Id}. {w.Text} {w.TranslatesList[0].Text} - {w.PhrasesList[0].PhraseText}");
                }
            }
            Console.Read();
        }




        static void Save()
        {
          var res =  File.ReadLines("d:\\words.txt")
                         .Where(l => l.Length > 0)
                         .Select(x => x.Split(new[] { ')' }, StringSplitOptions.RemoveEmptyEntries))
                         .Select(i => new WordDescription(i[0],i[1]))
                         .ToList();
            Console.WriteLine(res[0].Word);
            var wds = new Words(res);
            XmlSerializer formatter = new XmlSerializer(typeof(Words));
            using (FileStream fs = new FileStream("d:\\words.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, wds);

                Console.WriteLine("Список сериализован");
            }
        }

        static void SaveTranslates(List<TranslatedResponse> list)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<TranslatedResponse>));
            using (FileStream fs = new FileStream("d:\\words.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, list);

                Console.WriteLine("Список сериализован");
            }
        }
            static Words Read()
        {
            var wds = new Words();
            XmlSerializer formatter = new XmlSerializer(typeof(Words));
            using (FileStream fs = new FileStream("d:\\words.xml", FileMode.OpenOrCreate))
            {
                wds = (Words)formatter.Deserialize(fs);

                Console.WriteLine("Список десериализован");
            }
            return wds;
        }

        static List<TranslatedResponse> Request(Words words)
        {
            var responses = new List<TranslatedResponse>(); 
            foreach (var word in words.words)
            {
                 responses.Add (Do(word.Word).Result);
                Console.WriteLine(word.Word);
            }
            return responses;
        }

        static async Task<TranslatedResponse> Do(string wordName)
        {
            var service = new ReversoService();
            TranslatedResponse translatedWord = await service.TranslateWord(new TranslateWordRequest(from: Language.En, to: Language.Ru)
            {
                Word = wordName
            });
            return translatedWord;
        }
    }
}
