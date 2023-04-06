using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Model.Data.Repositories;
using Model.Services;
using ReversoApi.Models;

namespace MemorizerTests
{
    [TestClass]
    public class AllWordsControllerTests
    {
        private WebAppContext _context;
        private AllWordsService _controller;
        private GenericRepository<LearningWord> learning;
        private GenericRepository<Word> allRep;
        private AllWordsService allService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebAppContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
            _context = new WebAppContext(options);
            learning = new GenericRepository<LearningWord>(_context);
            allRep = new GenericRepository<Word>(_context);
            allService = new AllWordsService(allRep);
        }

        [TestMethod]
        public async Task FindWordByName_ShouldReturnWordWithName()
        {
            // Arrange
            var name = "Example";
            _context.Words.Add(new Word { Text = name });
            _context.SaveChanges();

            // Act
            var result =  await _controller.FindWordByName(name);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Word));
            Assert.AreEqual(name, result.Text);
        }

        [TestMethod]
        public async Task FindWordByName_ShouldReturnNullForInvalidName()
        {
            // Arrange
            var name = "";

            // Act
            var result = await _controller.FindWordByName(name);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async void FindWordsById_ShouldReturnListOfWords()
        {
            // Arrange
            var words = new List<Word> {
            new Word { Text = "word1" },
            new Word { Text = "word2" },
            new Word { Text = "word3" }
        };
            _context.Words.AddRange(words);
            _context.SaveChanges();

            // Act
            var result = await _controller.FindWordsById(2, 2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Word>));
            Assert.AreEqual(2, 2);
            Assert.AreEqual("word2", result[0].Text);
            Assert.AreEqual("word3", result[1].Text);
        }
    }

}