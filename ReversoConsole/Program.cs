using System;
using System.Threading.Tasks;
using ReversoApi.Models;
using ReversoApi.Models.Word;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ReversoConsole.DbModel;
using ReversoConsole.Controller;
using ReversoConsole.ConsoleCommands;

namespace ReversoApi
{
    [Serializable]
    public class WordDescription
    {
        public string ID { get; }
        public string Word { get; }
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
    public class NWords
    {
        public List<WordDescription> Words {get;}

        public NWords() 
        {
            this.Words = new List<WordDescription>();
        }

        public NWords(List<WordDescription> words)
        {
            this.Words = words;
        }
    }

    static class Program
    {

        private static readonly string txtfile = "c:\\plot\\3.txt";
        private static readonly string outputTxtfile = "c:\\plot\\2.txt";
        private static readonly string xmlfile = "d:\\words.xml";

        static void Migration()
        {
            var translates = ReadTranslates();
            var dbmodel = new List<Word>();
            foreach (var word in translates)
            {
                if (!word.Error && word.Success)
                {
                    var w = new Word
                    {
                        Text = word.Sources[0].DisplaySource
                    };
                    var items = ( from translate in word.Sources[0].Translations
                                select new Translate
                                {
                                    Text = translate.Translation
                                } ).ToList();
                    w.Translates = new List<Translate>();
                    w.Translates.AddRange(items);
                    dbmodel.Add(w);
                }
                else Console.WriteLine($"{word.Sources[0].DisplaySource }");
            }
            using var db = new ReversoConsole.Controller.WebAppContext();
            db.AddRange(dbmodel);
            db.SaveChanges();
            Console.WriteLine("Объекты успешно сохранены");

        }
        static async Task Main(string[] args)
        {

            Clear();
            Console.WriteLine("EnterName");
            var name = Console.ReadLine();
            //var userController = new UserController();                      
            var command = new CommandService();
            foreach (var c in command.Get())
            {
                Console.WriteLine( $"{c.Key} - {c.Value.Name}" );
            }

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                Console.Write("Enter command: >");
                //await command.Execute(userController, Console.ReadLine());
            }
        }

        static void Clear()
        {
            var res = File.ReadLines(txtfile);
                res = res.Select(x => x.Split(" ", 10).First()).ToList();
            File.WriteAllLines(outputTxtfile, res);  

        }

        static void Save()
        {
          var res =  File.ReadLines(txtfile)
                         .Where(l => l.Length > 0)
                         .Select(x => x.Split(new[] { ')' }, StringSplitOptions.RemoveEmptyEntries))
                         .Select(i => new WordDescription(i[0],i[1]))
                         .ToList();
            Console.WriteLine(res[0].Word);
            var wds = new NWords(res);
            XmlSerializer formatter = new XmlSerializer(typeof(NWords));
            using FileStream fs = new FileStream(xmlfile, FileMode.OpenOrCreate);
            formatter.Serialize(fs, wds);

            Console.WriteLine("Список сериализован");
        }

        static void SaveTranslates(List<TranslatedResponse> list)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<TranslatedResponse>));
            using FileStream fs = new FileStream(xmlfile, FileMode.OpenOrCreate);
            formatter.Serialize(fs, list);

            Console.WriteLine("Список сериализован");
        }
            static List<TranslatedResponse> ReadTranslates()
        {
            
            XmlSerializer formatter = new XmlSerializer(typeof(List<TranslatedResponse>));
            using FileStream fs = new FileStream(xmlfile, FileMode.OpenOrCreate);
            var wds = (List<TranslatedResponse>)formatter.Deserialize(fs);
            Console.WriteLine("Список десериализован");
            return wds;

        }

        static List<TranslatedResponse> Request(NWords words)
        {
            var responses = new List<TranslatedResponse>(); 
            foreach (var word in words.Words.Select(x => x.Word))
            {
                 responses.Add (Do(word).Result);
                Console.WriteLine(word);
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
