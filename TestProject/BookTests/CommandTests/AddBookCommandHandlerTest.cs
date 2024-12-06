using Application.ApplicationDtos;
using Application.Commands.Books.AddBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.CommandTests
{
   [TestFixture]
   public class AddBookCommandHandlerTest
   {
        private AddBookCommandHandler _handler;
        private IBookRepository _mockBookRepository;
        private ILogger<AddBookCommandHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mocka repositoryn
            _mockBookRepository = A.Fake<IBookRepository>();

            // Mocka loggern
            _mockLogger = A.Fake<ILogger<AddBookCommandHandler>>();

            // Skapa handlern med mockad repository
            _handler = new AddBookCommandHandler(_mockBookRepository, _mockLogger);
        }
        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenBookIsAdded()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Title = "Test Book",
                Description = "A great book",
                Author = new AuthorDto { Name = "Test Author" }
            };

            var command = new AddBookCommand(bookDto);

            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                Description = "A great book",
                Author = new Author { Name = "Test Author" }
            };

            // Mocka så att repositoryt returnerar en lyckad operation
            A.CallTo(() => _mockBookRepository.AddBook(A<BookDto>._))
                .Returns(Task.FromResult(OperationResult<Book>.Success(book, "Book added successfully.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Book added successfully.", result.Message);
            Assert.AreEqual("Test Book", result.Data.Title);
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookDataIsInvalid()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Title = string.Empty, // Ogiltigt titel
                Description = "A great book",
                Author = new AuthorDto { Name = "Test Author" }
            };

            var command = new AddBookCommand(bookDto);

            // Mocka så att repositoryt returnerar ett misslyckande
            A.CallTo(() => _mockBookRepository.AddBook(A<BookDto>._))
                .Returns(Task.FromResult(OperationResult<Book>.Failure("Invalid book data", "Title is required.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to add book.", result.ErrorMessage);
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Title = "Valid Book",
                Description = "A great book",
                Author = new AuthorDto { Name = "Test Author" }
            };

            var command = new AddBookCommand(bookDto);

            // Mocka så att repositoryt returnerar ett generellt misslyckande
            A.CallTo(() => _mockBookRepository.AddBook(A<BookDto>._))
                .Returns(Task.FromResult(OperationResult<Book>.Failure("An error occurred", "Database error.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to add book.", result.ErrorMessage);
        }
    }
}
