using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetAll;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.QueryTests
{
    [TestFixture]
    public class GetAllBooksQueryHandlerTests
    {
        private GetAllBooksQueryHandler _handler;
        private IBookRepository _mockBookRepository;
        private ILogger<GetAllBooksQueryHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mocka repository
            _mockBookRepository = A.Fake<IBookRepository>();

            // Mocka logger
            _mockLogger = A.Fake<ILogger<GetAllBooksQueryHandler>>();

            // Skapa handlern med mockad repository
            _handler = new GetAllBooksQueryHandler(_mockBookRepository, _mockLogger);
        }

        [Test]
        public async Task Handle_ShouldReturnBooks_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Description = "Description 1" },
            new Book { Id = 2, Title = "Book 2", Description = "Description 2" }
        };

            var command = new GetAllBooksQuery();

            // Mocka så att repositoryt returnerar en lista av böcker
            A.CallTo(() => _mockBookRepository.GetAllBooks())
                .Returns(Task.FromResult(OperationResult<List<Book>>.Success(books, "Books retrieved successfully.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual("Books retrieved successfully.", result.Message);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenNoBooksExist()
        {
            // Arrange
            var command = new GetAllBooksQuery();

            // Mocka så att repositoryt returnerar ett misslyckande
            A.CallTo(() => _mockBookRepository.GetAllBooks())
                .Returns(Task.FromResult(OperationResult<List<Book>>.Failure("No books found.", "Database error.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to retrieve books.", result.ErrorMessage);
        }

        
    }
}
