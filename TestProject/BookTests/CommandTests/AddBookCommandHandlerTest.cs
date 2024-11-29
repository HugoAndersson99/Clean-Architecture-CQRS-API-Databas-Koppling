using Application.Commands.Books.AddBook;
using Domain;
using Infrastructure.Database;

namespace TestProject.BookTests.CommandTests
{
    [TestFixture]
    public class AddBookCommandHandlerTest
    {
        private FakeDatabase _fakeDatabase;
        private AddBookCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new AddBookCommandHandler(_fakeDatabase);
        }
        [Test]
        public async Task Handle_ShouldAddBookToDatabase()
        {
            // Arrange
            int newId = 10;
            var newBook = new Book(newId, "New Book", "New Description");
            var command = new AddBookCommand(newBook);
            int previousCountInDatabase = _fakeDatabase.Books.Count;
            int newCountInDatabase = previousCountInDatabase + 1;

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.Contains(newBook));
            Assert.AreEqual(newCountInDatabase, _fakeDatabase.Books.Count);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenNewBookIsNull()
        {
            // Arrange
            var command = new AddBookCommand(null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
        [Test]
        public void Handle_ShouldThrowException_WhenCommandIsNull()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _handler.Handle(null, CancellationToken.None));
        }
        [Test]
        public async Task Handle_ShouldAddBookWithNewAuthor_WhenAuthorDoesNotExist()
        {
            // Arrange
            var newAuthor = new Author(10, "New Author");
            var newBook = new Book(10, "New Book", "New Description", newAuthor);
            var command = new AddBookCommand(newBook);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Contains.Item(newBook));
            Assert.That(_fakeDatabase.Authors, Contains.Item(newAuthor));
            Assert.That(newAuthor.Books, Contains.Item(newBook));
        }
        [Test]
        public async Task Handle_ShouldThrowException_WhenBookAlreadyExists()
        {
            // Arrange
            var existingBook = new Book(10, "Existing Book", "Existing Description");
            _fakeDatabase.Books.Add(existingBook);

            var command = new AddBookCommand(existingBook);

            // Act & Assert
            var exceptionMessage = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(exceptionMessage.Message, Is.EqualTo($"A book with Id {existingBook.Id} already exists."));
        }
        [Test]
        public async Task Handle_ShouldAddBookWithoutAuthor_WhenAuthorIsNull()
        {
            // Arrange
            var newBook = new Book(0, "Authorless Book", "Description", null);
            var command = new AddBookCommand(newBook);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Contains.Item(newBook));
            Assert.That(newBook.Author, Is.Null);
        }
    }
}
