using Memorizer.Algorithm;
using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Model.Data.Repositories;
using Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizerTests
{
    [TestClass]
    public class StandardLessonTests
    {
        private MyUserService user;
        private WebAppContext context;
        private GenericRepository<LearningWord> learning;

        [TestInitialize]
        public void Initialize()
        {
          //  user = new MyUserService();
            var options = new DbContextOptionsBuilder<WebAppContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
            learning = new GenericRepository<LearningWord>(context);
            context = new WebAppContext(options);
            context.LearningWords.Add(new LearningWord { UserId = 1, WordToLearnId = 1, Level = 0 });
            context.LearningWords.Add(new LearningWord { UserId = 1, WordToLearnId = 2, Level = 1 });
            context.LearningWords.Add(new LearningWord { UserId = 1, WordToLearnId = 3, Level = 2 });
            context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task GetNextLesson_ReturnsLessonWithCorrectNumberOfWords()
        {
            var lessonSettings = new LessonSetings { NewWordsInLesson = 1, WordsInLesson = 2 };
            var standardLesson = new StandardLesson(user, learning);
            var nextLesson = await standardLesson.GetNextLesson();

            Assert.AreEqual(2, nextLesson.WordsList.Count);
        }

        [TestMethod]
        public async Task GetNextLesson_ReturnsLessonWithCorrectNewWords()
        {
            var lessonSettings = new LessonSetings { NewWordsInLesson = 1, WordsInLesson = 2 };
            var standardLesson = new StandardLesson(user, learning);
            var nextLesson = await standardLesson.GetNextLesson();

            Assert.IsTrue( nextLesson.WordsList.Any(x => x.LearningWord.Level == 0));
        }

        [TestMethod]
        public async Task GetNextLesson_ReturnsLessonWithCorrectRepeatedWords()
        {
            var lessonSettings = new LessonSetings { NewWordsInLesson = 1, WordsInLesson = 2 };
            var standardLesson = new StandardLesson(user, learning);
            var nextLesson = await standardLesson.GetNextLesson();

            Assert.IsTrue(nextLesson.WordsList.Any(x => x.LearningWord.Level > 0));
        }

        [TestMethod]
        public async Task ReturnFinishedLesson_UpdatesWordLevelsCorrectly()
        {
            var lessonSettings = new LessonSetings { maxLevel = 3 };
            var standardLesson = new StandardLesson(user, learning);
            var lesson = new Lesson
            {
                WordsList = new List<LessonWord> {
            new LessonWord { LearningWord = context.LearningWords.Find(1), IsSuccessful = IsSuccessful.True },
            new LessonWord { LearningWord = context.LearningWords.Find(2), IsSuccessful = IsSuccessful.False },
            new LessonWord { LearningWord = context.LearningWords.Find(3), IsSuccessful = IsSuccessful.Finished }
        }
            };

            await standardLesson.ReturnFinishedLesson(lesson);

            Assert.AreEqual(1, context.LearningWords.Find(1).Level);
            Assert.AreEqual(1, context.LearningWords.Find(2).Level);
            Assert.AreEqual(-1, context.LearningWords.Find(3).Level);
        }
        [TestMethod]
        public async Task ReturnFinishedLesson_SavesChangesToContext()
        {
            var lessonSettings = new LessonSetings { maxLevel = 3 };
            var standardLesson = new StandardLesson(user, learning);
            var lesson = new Lesson
            {
                WordsList = new List<LessonWord> {
        new LessonWord { LearningWord = context.LearningWords.Find(1), IsSuccessful = IsSuccessful.True },
        new LessonWord { LearningWord = context.LearningWords.Find(2), IsSuccessful = IsSuccessful.False },
        new LessonWord { LearningWord = context.LearningWords.Find(3), IsSuccessful = IsSuccessful.Finished }
    }
            };

            await standardLesson.ReturnFinishedLesson(lesson);

            Assert.IsTrue(context.ChangeTracker.HasChanges());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetNextLesson_ThrowsExceptionWhenNoWordsToLearn()
        {
            context.LearningWords.Remove(context.LearningWords.Find(1));
            context.LearningWords.Remove(context.LearningWords.Find(2));
            context.LearningWords.Remove(context.LearningWords.Find(3));

            var standardLesson = new StandardLesson(user, learning);
            await standardLesson.GetNextLesson();
        }
    }
}

