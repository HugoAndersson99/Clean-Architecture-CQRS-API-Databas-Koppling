using Application.Commands.Books.DeleteBook;
using Domain;
using Infrastructure.Database;

namespace TestProject.BookTests.CommandTests
{
    [TestFixture]
    public class DeleteBookCommandHandlerTests
    {
        private DeleteBookCommandHandler _handler;
        private FakeDatabase _fakeDatabase;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new DeleteBookCommandHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            int bookId = 10;
            Book bookToDelete = new Book(bookId, "Title", "Description");
            _fakeDatabase.Books.Add(bookToDelete);
            var command = new DeleteBookCommand(bookId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Does.Not.Contain(bookToDelete));
            Assert.That(_fakeDatabase.Books, Does.Not.Contain(bookToDelete));
        }

        [Test]
        public void Handle_ShouldThrowArgumentException_WhenIdIsInvalid()
        {
            // Arrange
            int invalidId = 0;
            var command = new DeleteBookCommand(invalidId);

            // Act & Assert
            var exceptionReturn = Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(exceptionReturn.Message, Does.Contain("Id must be greater than 0."));
        }
        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            int nonExistentId = 999;
            var command = new DeleteBookCommand(nonExistentId);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(ex.Message, Is.EqualTo($"No book found with Id {nonExistentId}."));
        }
        [Test]
        public async Task Handle_ShouldReturnUpdatedList_WhenBookIsDeleted()
        {
            // Arrange
            int bookIdToBeDeleted = 10;
            int bookIdToStay = 20;
            var book1 = new Book(bookIdToBeDeleted, "Title1", "Description1");
            var book2 = new Book(bookIdToStay, "Title2", "Description2");
            _fakeDatabase.Books.Add(book1);
            _fakeDatabase.Books.Add(book2);
            int bookCountBeforeDelete = _fakeDatabase.Books.Count;
            int newBookCountAfterDelete = _fakeDatabase.Books.Count - 1;
            var command = new DeleteBookCommand(bookIdToBeDeleted);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Count, Is.EqualTo(newBookCountAfterDelete));
            Assert.That(result, Does.Contain(book2));
            Assert.That(result, Does.Not.Contain(book1));
        }
    }
}
