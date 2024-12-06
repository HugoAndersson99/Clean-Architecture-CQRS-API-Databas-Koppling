using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetById;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.QueryTests
{
    [TestFixture]
    public class GetBookByIdQueryHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<ILogger<GetBookByIdQueryHandler>> _loggerMock;
        private GetBookByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<GetBookByIdQueryHandler>>();
            _handler = new GetBookByIdQueryHandler(_bookRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book 1", Description = "Description 1", Author = new Author { Id = 1, Name = "Author 1" } };
            var command = new GetBookByIdQuery(1);

            _bookRepositoryMock
                .Setup(repo => repo.GetBookById(1))
                .ReturnsAsync(OperationResult<Book>.Success(book, "Book retrieved successfully."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Book retrieved successfully.", result.Message);
            Assert.AreEqual(1, result.Data.Id);
            _bookRepositoryMock.Verify(repo => repo.GetBookById(1), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookNotFound()
        {
            // Arrange
            var command = new GetBookByIdQuery(99);  // Non-existing book ID

            _bookRepositoryMock
                .Setup(repo => repo.GetBookById(99))
                .ReturnsAsync(OperationResult<Book>.Failure("No book found with the given Id.", "Database error."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("No book found with the given Id.", result.Message);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
        {
            // Arrange
            var command = new GetBookByIdQuery(1);

            _bookRepositoryMock
                .Setup(repo => repo.GetBookById(1))
                .ReturnsAsync(OperationResult<Book>.Failure("An error occurred while retrieving the book."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("An error occurred while retrieving the book.", result.Message);
        }
    }
}
