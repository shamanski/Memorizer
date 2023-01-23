using Memorizer.Controller;
using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;

namespace MemorizerTests
{
    [TestClass]
    public class LearningControllerTests
    {
        private WebAppContext _context;
        private LearningController _controller;
        private User _user;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WebAppContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
            _context = new WebAppContext(options);
            _user = new User() { Id = 1, Name = "Vasya" };
            _controller = new LearningController(_user, _context);
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
            var word = new LearningWord(_user, new Word { Text = "TestCase", Id = 888 }) { Id=999};
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
            var word = new LearningWord(_user, new Word { Text = "test" });

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
            var word = new LearningWord(_user, new Word { Text = "test" });
            _context.LearningWords.Add(word);

            // Act
            var result = _controller.RemoveWord(word);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_context.LearningWords.Count(), 2);
        }
    }
}
