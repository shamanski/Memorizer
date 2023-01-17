using ReversoConsole.Controller;
using ReversoConsole.DbModel;

namespace MemorizerTests
{
    [TestClass]
    public class AllWordsControllerTests
    {
        private WebAppContext _context;
        private AllWordsController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            _context = new WebAppContext();
            _controller = new AllWordsController(_context);
        }

        [TestMethod]
        public void GetAllWords_ShouldReturnAllWordsInContext()
        {
            // Act
            var result = _controller.GetAllWords();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Word>));
            Assert.AreEqual(_context.Words.Count(), result.Count);
        }

        [TestMethod]
        public void FindWordByName_ShouldReturnWordWithName()
        {
            // Arrange
            var name = "example";
            _context.Words.Add(new Word { Text = name });
            _context.SaveChanges();

            // Act
            var result = _controller.FindWordByName(name);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Word));
            Assert.AreEqual(name, result.Text);
        }

        [TestMethod]
        public void FindWordByName_ShouldReturnNullForInvalidName()
        {
            // Arrange
            var name = "";

            // Act
            var result = _controller.FindWordByName(name);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindWordsById_ShouldReturnListOfWords()
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
            var result = _controller.FindWordsById(1, 2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Word>));
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("word2", result[0].Text);
            Assert.AreEqual("word3", result[1].Text);
        }
    }

}