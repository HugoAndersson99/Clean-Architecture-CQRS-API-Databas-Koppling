using Application.ApplicationDtos;
using Application.Commands.Books.UpdateBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.CommandTests
{
    [TestFixture]
    public class UpdateBookByIdCommandHandlerTests
    {
        private UpdateBookByIdCommandHandler _handler;
        private IBookRepository _mockBookRepository;
        private ILogger<UpdateBookByIdCommandHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mocka repositoryn
            _mockBookRepository = A.Fake<IBookRepository>();

            // Mocka loggern
            _mockLogger = A.Fake<ILogger<UpdateBookByIdCommandHandler>>();

            // Skapa handlern med mockad repository
            _handler = new UpdateBookByIdCommandHandler(_mockBookRepository, _mockLogger);
        }
        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenBookIsUpdated()
        {
            // Arrange
            var bookId = 1; // Exempel på ID för bok
            var updatedBookDto = new BookDto
            {
                Title = "Updated Book Title",
                Description = "Updated description"
            };
            var command = new UpdateBookByIdCommand(bookId, updatedBookDto);

            // Mocka så att repositoryt returnerar en lyckad uppdatering
            A.CallTo(() => _mockBookRepository.UpdateBook(bookId, updatedBookDto))
                .Returns(Task.FromResult(OperationResult<Book>.Success(new Book { Id = bookId, Title = "Updated Book Title" }, "Book updated successfully.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Book updated successfully.", result.Message);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 1; // Exempel på ID för bok som inte finns
            var updatedBookDto = new BookDto
            {
                Title = "Updated Book Title",
                Description = "Updated description"
            };
            var command = new UpdateBookByIdCommand(bookId, updatedBookDto);

            // Mocka så att repositoryt returnerar ett misslyckande (bok inte funnen)
            A.CallTo(() => _mockBookRepository.UpdateBook(bookId, updatedBookDto))
                .Returns(Task.FromResult(OperationResult<Book>.Failure("No book found with the given ID.", "Database error.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to update book.", result.ErrorMessage);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
        {
            // Arrange
            var bookId = 1; // Exempel på ID för bok
            var updatedBookDto = new BookDto
            {
                Title = "Updated Book Title",
                Description = "Updated description"
            };
            var command = new UpdateBookByIdCommand(bookId, updatedBookDto);

            // Mocka så att repositoryt returnerar ett generellt fel
            A.CallTo(() => _mockBookRepository.UpdateBook(bookId, updatedBookDto))
                .Returns(Task.FromResult(OperationResult<Book>.Failure("An error occurred", "Database error.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to update book.", result.ErrorMessage);
        }
    }
}
