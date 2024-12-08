using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAllAuthors;
using Application.Queries.Authors.GetAuthorById;
using Domain;
using FakeItEasy;
using Infrastructure.Database;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.AuthorTests.AuthorQueryTests
{
    [TestFixture]
    public class GetAuthorByIdQueryHandlerTests
    {
        private GetAuthorByIdQueryHandler _handler;
        private IAuthorRepository _mockAuthorRepository;
        private ILogger<GetAuthorByIdQueryHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockAuthorRepository = A.Fake<IAuthorRepository>();

            _mockLogger = A.Fake<ILogger<GetAuthorByIdQueryHandler>>();

            var _mochCache = A.Fake<IMemoryCache>();

            _handler = new GetAuthorByIdQueryHandler(_mockAuthorRepository, _mockLogger, _mochCache);
        }
        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorExists()
        {
            // Arrange
            var command = new GetAuthorByIdQuery(1);

            var author = new Author { Id = 1, Name = "Test Author" };
            var mockResult = OperationResult<Author>.Success(author, "Author found successfully.");

            A.CallTo(() => _mockAuthorRepository.GetAuthorById(1))
                .Returns(Task.FromResult(mockResult));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Author found successfully.", result.Message);
            Assert.AreEqual("Test Author", result.Data.Name);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorNotFound()
        {
            // Arrange
            int nonExistingId = 999;
            var command = new GetAuthorByIdQuery(nonExistingId);

            var mockResult = OperationResult<Author>.Failure("Author not found.", "No author with this ID.");

            A.CallTo(() => _mockAuthorRepository.GetAuthorById(nonExistingId))
                .Returns(Task.FromResult(mockResult));

            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Author not found.", result.ErrorMessage);
        }
    }
}
