using Application.Commands.Books.DeleteBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.CommandTests
{
    [TestFixture]
    public class DeleteBookCommandHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<ILogger<DeleteBookCommandHandler>> _loggerMock;
        private DeleteBookCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<DeleteBookCommandHandler>>();
            _handler = new DeleteBookCommandHandler(_bookRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            var bookId = 1;
            var command = new DeleteBookCommand(bookId);

            _bookRepositoryMock
                .Setup(repo => repo.DeleteBook(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<Book>.Success(null, "Book deleted successfully."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Book deleted successfully.", result.Message);
            _bookRepositoryMock.Verify(repo => repo.DeleteBook(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 999; // Assume this ID does not exist
            var command = new DeleteBookCommand(bookId);

            _bookRepositoryMock
                .Setup(repo => repo.DeleteBook(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<Book>.Failure("Book not found.", "Database error."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Book not found.", result.Message);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
        {
            // Arrange
            var bookId = 1;
            var command = new DeleteBookCommand(bookId);

            _bookRepositoryMock
                .Setup(repo => repo.DeleteBook(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<Book>.Failure("An error occurred while deleting the book."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("An error occurred while deleting the book.", result.Message);
        }
    }
}
