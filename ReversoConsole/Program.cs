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
            TranslatedResponse response = Do().Result;
            XmlSerializer formatter = new XmlSerializer(typeof(TranslatedResponse));          
            using (FileStream fs = new FileStream("d:\\persons.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, response);

                Console.WriteLine("Объект сериализован");
            }
            Read();
            Console.ReadLine();

        }


        static void Read()
        {
          var res =  File.ReadLines("d:\\words.txt").Where(l => l.Length > 0).Select(x => x.Split(new[] { ')' }, StringSplitOptions.RemoveEmptyEntries))
    .Select(i => new WordDescription(i[0],i[1])).ToList();
            Console.WriteLine(res[0].Word);
            var wds = new Words(res);
            XmlSerializer formatter = new XmlSerializer(typeof(Words));
            using (FileStream fs = new FileStream("d:\\words.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, wds);

                Console.WriteLine("Список сериализован");
            }
        }

        static async Task<TranslatedResponse> Do()
        {
            var service = new ReversoService();
            TranslatedResponse translatedWord = await service.TranslateWord(new TranslateWordRequest(from: Language.En, to: Language.Ru)
            {
                Word = "table"
            });
            return translatedWord;
        }
    }
}
