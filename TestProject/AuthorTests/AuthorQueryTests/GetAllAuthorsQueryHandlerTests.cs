using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAllAuthors;
using Domain;
using FakeItEasy;
using Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.AuthorTests.AuthorQueryTests
{
    [TestFixture]
    public class GetAllAuthorsQueryHandlerTests
    {
        private GetAllAuthorsQueryHandler _handler;
        private IAuthorRepository _mockAuthorRepository;
        private ILogger<GetAllAuthorsQueryHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mocka repositoryn och loggaren
            _mockAuthorRepository = A.Fake<IAuthorRepository>();
            _mockLogger = A.Fake<ILogger<GetAllAuthorsQueryHandler>>();

            // Skapa handlern med mockad repository och logger
            _handler = new GetAllAuthorsQueryHandler(_mockAuthorRepository, _mockLogger);
        }
        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorsExist()
        {
            // Arrange
            var command = new GetAllAuthorsQuery(); // Din query-klass

            // Mocka repositoryt för att returnera en lista med författare
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author 1" },
                new Author { Id = 2, Name = "Author 2" }
            };

            A.CallTo(() => _mockAuthorRepository.GetAllAuthors())
                .Returns(Task.FromResult(OperationResult<List<Author>>.Success(authors)));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual("Author 1", result.Data[0].Name);
            Assert.AreEqual("Author 2", result.Data[1].Name);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenNoAuthorsExist()
        {
            // Arrange
            var command = new GetAllAuthorsQuery(); // Din query-klass

            // Mocka repositoryt för att returnera en tom lista
            A.CallTo(() => _mockAuthorRepository.GetAllAuthors())
                .Returns(Task.FromResult(OperationResult<List<Author>>.Failure("No authors found.", "Database is empty.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("No authors found.", result.ErrorMessage);
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var command = new GetAllAuthorsQuery(); // Din query-klass

            // Mocka repositoryt för att kasta ett undantag
            A.CallTo(() => _mockAuthorRepository.GetAllAuthors())
                .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("An error occurred while fetching authors.", result.ErrorMessage);
        }

    }
}
