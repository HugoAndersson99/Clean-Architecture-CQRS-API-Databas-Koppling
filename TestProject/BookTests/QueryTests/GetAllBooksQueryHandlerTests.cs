using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetAll;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.QueryTests
{
    [TestFixture]
    public class GetAllBooksQueryHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<ILogger<GetAllBooksQueryHandler>> _loggerMock;
        private GetAllBooksQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<GetAllBooksQueryHandler>>();
            _handler = new GetAllBooksQueryHandler(_bookRepositoryMock.Object, _loggerMock.Object);
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

            _bookRepositoryMock
                .Setup(repo => repo.GetAllBooks())
                .ReturnsAsync(OperationResult<List<Book>>.Success(books, "Books retrieved successfully."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Books retrieved successfully.", result.Message);
            Assert.AreEqual(2, result.Data.Count);
            _bookRepositoryMock.Verify(repo => repo.GetAllBooks(), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenNoBooksFound()
        {
            // Arrange
            var command = new GetAllBooksQuery();

            _bookRepositoryMock
                .Setup(repo => repo.GetAllBooks())
                .ReturnsAsync(OperationResult<List<Book>>.Failure("No books found.", "Database error."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("No books found.", result.Message);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
        {
            // Arrange
            var command = new GetAllBooksQuery();

            _bookRepositoryMock
                .Setup(repo => repo.GetAllBooks())
                .ReturnsAsync(OperationResult<List<Book>>.Failure("An error occurred while retrieving books."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("An error occurred while retrieving books.", result.Message);
        }
    }
}
