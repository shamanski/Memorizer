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
        //start
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
        public List<WordDescription> Words { get; }

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

        //Run this if you want console application instead of webserver
        static async Task Main(string[] args)
        {

            DatabaseMakerAssistant.Clear();
            Console.WriteLine("EnterName");
            var name = Console.ReadLine();
            //var userController = new UserController();                      
            var command = new CommandService();
            foreach (var c in command.Get())
            {
                Console.WriteLine($"{c.Key} - {c.Value.Name}");
            }

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                Console.Write("Enter command: >");
                //await command.Execute(userController, Console.ReadLine());
            }
        }

        static List<TranslatedResponse> Request(NWords words)
        {
            var responses = new List<TranslatedResponse>();
            foreach (var word in words.Words.Select(x => x.Word))
            {
                responses.Add(Do(word).Result);
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
