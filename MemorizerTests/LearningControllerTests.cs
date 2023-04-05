using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Model.Data.Repositories;
using Model.Services;

namespace MemorizerTests
{
    [TestClass]
    public class LearningControllerTests
    {
        private WebAppContext _context;
        private LearningService _controller;
        private UserService _user;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebAppContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
            _context = new WebAppContext(options);
            //_user = new User("ff");
            var rep = new GenericRepository<LearningWord>(_context);
            _controller = new LearningService(_user, rep);
        }

        [TestMethod]
        public void GetAll_Returns_List_Of_LearningWords()
        {
            // Arrange
            TestInitialize();

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(List<LearningWord>));
        }

        [TestMethod]
        public void Find_Returns_LearningWord_With_Matching_Text()
        {
            // Arrange
            TestInitialize();
            var word = new LearningWord();
            _context.LearningWords.Add(word);
            // Act
            var result = _controller.Find("TestCase");

            // Assert
            Assert.IsInstanceOfType(result, typeof(LearningWord));
            Assert.AreEqual(result.WordToLearn.Text, "TestCase");
        }

        [TestMethod]
        public void AddNewWord_Adds_New_Word_To_LearningWords()
        {
            // Arrange
            TestInitialize();
            var word = new LearningWord();

            // Act
            var result = _controller.AddNewWord(word);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_context.LearningWords.Count(), 1);
            Assert.AreEqual(_context.LearningWords.First().WordToLearn.Text, "test");
        }

        [TestMethod]
        public void AddNewWords_Adds_Multiple_Words_To_LearningWords()
        {
            // Arrange
            TestInitialize();
            var words = new List<Word> {
            new Word { Text = "test1" },
            new Word { Text = "test2" }
        };

            // Act
            var result = _controller.AddNewWords(words);

            // Assert
            Assert.AreEqual(result, 2);
            Assert.AreEqual(_context.LearningWords.Count(), 2);
        }

        [TestMethod]
        public void RemoveWord_Removes_Word_From_LearningWords()
        {
            // Arrange
            TestInitialize();
            var word = new LearningWord();
            _context.LearningWords.Add(word);

            // Act
            var result = _controller.RemoveWord(word);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_context.LearningWords.Count(), 2);
        }
    }
}
