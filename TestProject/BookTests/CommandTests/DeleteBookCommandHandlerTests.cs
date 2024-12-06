using Application.Commands.Books.DeleteBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.CommandTests
{
    [TestFixture]
    public class DeleteBookCommandHandlerTests
    {
        private DeleteBookCommandHandler _handler;
        private IBookRepository _mockBookRepository;
        private ILogger<DeleteBookCommandHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mocka repositoryn
            _mockBookRepository = A.Fake<IBookRepository>();

            // Mocka loggern
            _mockLogger = A.Fake<ILogger<DeleteBookCommandHandler>>();

            // Skapa handlern med mockad repository
            _handler = new DeleteBookCommandHandler(_mockBookRepository, _mockLogger);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenBookIsDeleted()
        {
            // Arrange
            var bookId = 1; // Exempel på ID för bok
            var command = new DeleteBookCommand(bookId);

            // Mocka så att repositoryt returnerar en lyckad operation vid borttagning
            A.CallTo(() => _mockBookRepository.DeleteBook(bookId))
                .Returns(Task.FromResult(OperationResult<Book>.Success(null, "Book deleted successfully.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Book deleted successfully.", result.Message);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 1; // Exempel på ID för bok som inte finns
            var command = new DeleteBookCommand(bookId);

            // Mocka så att repositoryt returnerar ett misslyckande (bok inte funnen)
            A.CallTo(() => _mockBookRepository.DeleteBook(bookId))
                .Returns(Task.FromResult(OperationResult<Book>.Failure("Book not found.", "Database error.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to delete book.", result.ErrorMessage);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
        {
            // Arrange
            var bookId = 1; // Exempel på ID för bok
            var command = new DeleteBookCommand(bookId);

            // Mocka så att repositoryt returnerar ett generellt fel
            A.CallTo(() => _mockBookRepository.DeleteBook(bookId))
                .Returns(Task.FromResult(OperationResult<Book>.Failure("An error occurred", "Database error.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to delete book.", result.ErrorMessage);
        }
    }
}
