using Application.Commands.Books.UpdateBook;
using Domain;
using Infrastructure.Database;

namespace TestProject.BookTests.CommandTests
{
    [TestFixture]
    public class UpdateBookByIdCommandHandlerTests
    {
        private UpdateBookByIdCommandHandler _handler;
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new UpdateBookByIdCommandHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldUpdateBookDetails_WhenBookExists()
        {
            // Arrange
            var existingBook = new Book(1, "Old Title", "Old Description");
            _fakeDatabase.Books.Add(existingBook);

            var updatedBook = new Book(0, "New Title", "New Description");
            var command = new UpdateBookByIdCommand(updatedBook, 1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedBookInDb = _fakeDatabase.Books.FirstOrDefault(book => book.Id == 1);
            Assert.NotNull(updatedBookInDb);
            Assert.That(updatedBookInDb.Title, Is.EqualTo("New Title"));
            Assert.That(updatedBookInDb.Description, Is.EqualTo("New Description"));
            Assert.That(result, Contains.Item(updatedBookInDb));
        }
        [Test]
        public void Handle_ShouldThrowArgumentException_WhenIdIsInvalid()
        {
            // Arrange
            var updatedBook = new Book(0, "New Title", "New Description");
            var command = new UpdateBookByIdCommand(updatedBook, 0);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(ex.Message, Does.Contain("Id must be greater than 0."));
        }
        [Test]
        public void Handle_ShouldThrowArgumentNullException_WhenUpdatedBookIsNull()
        {
            // Arrange
            Book nullBook = null;
            var command = new UpdateBookByIdCommand(nullBook, 1);

            // Act & Assert
            var exceptionReturn = Assert.ThrowsAsync<ArgumentNullException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(exceptionReturn.Message, Does.Contain("Updated book details must be provided."));
        }

        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            int NonExistentBookId = 999;
            var updatedBook = new Book(0, "New Title", "New Description");
            var command = new UpdateBookByIdCommand(updatedBook, NonExistentBookId); // Non-existent book ID

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(ex.Message, Is.EqualTo($"Book with Id {NonExistentBookId} not found."));
        }
    }
}
