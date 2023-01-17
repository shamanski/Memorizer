using ReversoApi.Models;
using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ReversoApi
{
    internal static class DatabaseMakerAssistant
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
                    var items = (from translate in word.Sources[0].Translations
                                 select new Translate
                                 {
                                     Text = translate.Translation
                                 }).ToList();
                    w.Translates = new List<Translate>();
                    w.Translates.AddRange(items);
                    dbmodel.Add(w);
                }
                else Console.WriteLine($"{word.Sources[0].DisplaySource }");
            }
            using var db = new Memorizer.Controller.WebAppContext();
            db.AddRange(dbmodel);
            db.SaveChanges();
            Console.WriteLine("Объекты успешно сохранены");

        }
        static List<TranslatedResponse> ReadTranslates()
        {

            XmlSerializer formatter = new XmlSerializer(typeof(List<TranslatedResponse>));
            using FileStream fs = new FileStream(xmlfile, FileMode.OpenOrCreate);
            var wds = (List<TranslatedResponse>)formatter.Deserialize(fs);
            Console.WriteLine("Список десериализован");
            return wds;

        }

        public static void Save()
        {
            var res = File.ReadLines(txtfile)
                           .Where(l => l.Length > 0)
                           .Select(x => x.Split(new[] { ')' }, StringSplitOptions.RemoveEmptyEntries))
                           .Select(i => new WordDescription(i[0], i[1]))
                           .ToList();
            Console.WriteLine(res[0].Word);
            var wds = new NWords(res);
            XmlSerializer formatter = new XmlSerializer(typeof(NWords));
            using FileStream fs = new FileStream(xmlfile, FileMode.OpenOrCreate);
            formatter.Serialize(fs, wds);

            Console.WriteLine("Список сериализован");
        }

       public static void SaveTranslates(List<TranslatedResponse> list)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<TranslatedResponse>));
            using FileStream fs = new FileStream(xmlfile, FileMode.OpenOrCreate);
            formatter.Serialize(fs, list);

            Console.WriteLine("Список сериализован");
        }

        public static void Clear()
        {
            var res = File.ReadLines(txtfile);
            res = res.Select(x => x.Split(" ", 10).First()).ToList();
            File.WriteAllLines(outputTxtfile, res);

        }
    }
}