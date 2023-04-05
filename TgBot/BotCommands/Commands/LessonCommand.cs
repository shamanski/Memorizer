using Memorizer.Algorithm;
using Model.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Keybords;
using User = Memorizer.DbModel.User;

namespace TgBot.BotCommands.Commands
{
    [Command(Description = "/lesson - Начать урок")]
    public class LessonCommand : BotCommand, IBotCommand
    {
        private const string positive = "\u2705";
        private const string negative = "\u274C";
        public override string Name { get; } = "/lesson";
        private readonly ILessonService<Lesson> lessonService;
        private readonly LearningService learning;
        public Lesson lesson;
        int count = 0;

        public LessonCommand(ChatController chatController, LearningService learning, StandardLesson lessonService) : base(chatController)
        {
            this.lessonService = lessonService;
            this.learning = learning;

        }

        public async override Task<bool> Execute(User user, Message message, params string[] param)
        {
            if (learning.Count() < 10)
            {
                message.Text = "Добавьте хотя бы 10 слов для изучения. Можно использовать /startpack";
                await chat.ReplyMessage(message);
                return false;
            }

            lesson = await lessonService.GetNextLesson();
            message.Text = "Урок начат"+ Environment.NewLine;            
            await chat.ReplyMessage(message);            
            await StepAnswer(message);
            return  true;
        }

        
        //Take new word and make keyboard
        private async Task StepAnswer(Message message)
        {
            var currentWord = lesson.WordsList[count].LearningWord.WordToLearn;
            message.Text = String.Join(", ", currentWord.Translates.Take(10));
            var additionalList = lesson.WordsList[count].AdditionalWords;
            var rnd = new Random();
            additionalList.Insert(rnd.Next(additionalList.Count), lesson.WordsList[count].LearningWord.WordToLearn.Text);
            message.ReplyMarkup = new LessonKeyboard(additionalList, currentWord.Text).Keyboard;
            await chat.ReplyMessage(message);  
        }
        
        
        //Compare input or button text with correct answer and mark it
        private async Task CheckBox(Message message, string currWord)
        {
            if (message.ReplyMarkup != null ) //button pressed
            {
                foreach (var i in message?.ReplyMarkup?.InlineKeyboard)
                {
                    foreach (var n in i)
                    {
                        if (!n.CallbackData.StartsWith('!')) // excluding special buttons
                        {
                            n.Text = n.Text.Remove(0, 1).Insert(0, currWord == n.Text[1..] 
                                ? positive 
                                : negative);
                        }
                    }
                }
                await chat.EditMessageAsync(message);
            }

            else //answer got as text
            {
                message.Text = currWord == message.Text ? message.Text.Insert(0, positive) : message.Text.Insert(0, negative);
                await chat.ReplyMessage(message);
            }
            
        }

        //Remove special symbols and upper case
        private string Clear(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
            if (!char.IsLetter(input[0])) input = input[1..];
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(input);
        } 
        
        public async override Task<bool> Next(Memorizer.DbModel.User user, Message message)
        {
            var currWord = lesson.WordsList[count].ToString();
            if (message.Text == "!rm") // Button 'already have known' pressed
            {
                lesson.WordsList[count].IsSuccessful = IsSuccessful.Finished;
            }
            else
            {
                message.Text = Clear(message.Text);
                lesson.WordsList[count].IsSuccessful = (currWord == message.Text) ? IsSuccessful.True : IsSuccessful.False;
            }
            
            count++;
            await CheckBox(message, currWord);
            if (count == lesson.WordsList.Count) //after last answer
            {                
                await Last(user, message);                
                return false;
            }

            await StepAnswer(message);
            return true;
        }

        private async Task Last(Memorizer.DbModel.User user, Message message)
        {
            await lessonService.ReturnFinishedLesson(lesson);
            int successful = (from i in lesson.WordsList
                              where i.IsSuccessful == IsSuccessful.True || i.IsSuccessful == IsSuccessful.Finished
                              select i).Count();
            message.Text = $"Урок окончен. Результат: { successful} из {lesson.WordsList.Count}";
            message.ReplyMarkup = null;
            await chat.ReplyMessage(message);
        }
    }
}
