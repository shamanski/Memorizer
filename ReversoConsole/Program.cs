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
using ReversoConsole.Algorithm;
using ReversoConsole.ConsoleCommands;

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
        static void Migration()
        {
            var translates = ReadTranslates();
            var dbmodel = new List<Word>();
            foreach (var word in translates)
            {
                if (!word.Error && word.Success)
                {
                    var w = new Word();
                    w.Text = word.Sources[0].DisplaySource;
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
            using (var db = new ReversoConsole.Controller.AppContext())
            {
                db.AddRange(dbmodel);
                db.SaveChanges();
                Console.WriteLine("Объекты успешно сохранены");
            }
            
        }
        static async Task Main(string[] args)
        {
            //Migration();
            //Console.WriteLine("input ID");
            //int id =Int32.Parse(Console.ReadLine());
            //using (var db = new ReversoConsole.Controller.AppContext())
            //{
            //    var w = db.Words.Include(u => u.TranslatesList);
            //    var n = (from word in w
            //            where word.Id == id
            //            select word).ToList();
            //    Console.WriteLine(n[0].Text);
            //    foreach (var translate in n[0].TranslatesList)
            //    {
            //        Console.Write($"{translate.Text} ");
            //    }
                
            //}

            Console.WriteLine("EnterName");
            var name = Console.ReadLine();

            var userController = new UserController(name);
            var standardLesson = new StandardLesson(userController.CurrentUser);
            var allWords = new AllWordsController();
            var learningController = new LearningController(userController.CurrentUser);
            //var exerciseController = new ExerciseController(userController.CurrentUser);
            if (userController.IsNewUser)
            {

                userController.SetNewUserData(name);
            }


            Console.WriteLine(userController.CurrentUser);



            while (true)
            {
                Console.WriteLine("Что вы хотите сделать?");
                Console.WriteLine("E - ввести слово");
                Console.WriteLine("A - урок");
                Console.WriteLine("Q - выход");
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.E:
                        Console.Write("Введите слово:");
                        string word = Console.ReadLine();
                        var makeWord = allWords.FindWordByName(word);
                        var newWord = new LearningWord(userController.CurrentUser, makeWord);
                        learningController.AddNewWord(newWord);

                        foreach (var item in learningController.Words)
                        {
                            Console.WriteLine($"\t{item.WordToLearn.Text} - {item.WordToLearn.Translates[0].Text}");
                        }
                        break;
                    case ConsoleKey.A:
                        var learning = new LearningController(userController.CurrentUser);
                        var lesson = learning.Lesson.GetNextLesson();
                        foreach (var currWord in lesson.WordsList)
                        {
                            Console.WriteLine(currWord.LearningWord.WordToLearn.Translates[0].Text);
                            if (Console.ReadLine() == currWord.LearningWord.WordToLearn.Text)
                            {
                                currWord.isSuccessful = IsSuccessful.True;
                                Console.WriteLine("Yes");
                            }
                            else
                            {
                                currWord.isSuccessful = IsSuccessful.False;
                                Console.WriteLine("No");
                            }
                                   
                        }
                        learning.Lesson.ReturnFinishedLesson(lesson);
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                }

                Console.ReadLine();
            }

        }

        static async void runCommand (User user, string message, ICommandService service)
        {
            foreach (var command in service.Get())
            {
                if (command.Contains(message))
                {
                    await command.Execute(user, message);
                    break;
                }
            }
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
            static List<TranslatedResponse> ReadTranslates()
        {
            var wds = new List<TranslatedResponse>();
            XmlSerializer formatter = new XmlSerializer(typeof(List<TranslatedResponse>));
            using (FileStream fs = new FileStream("d:\\words.xml", FileMode.OpenOrCreate))
            {
                wds = (List<TranslatedResponse>)formatter.Deserialize(fs);

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
