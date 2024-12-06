using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAllAuthors;
using Application.Queries.Authors.GetAuthorById;
using Domain;
using Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.AuthorTests.AuthorQueryTests
{
    [TestFixture]
    public class GetAuthorByIdQueryHandlerTests
    {
        private Mock<IAuthorRepository> _mockAuthorRepository;
        private Mock<ILogger<GetAuthorByIdQueryHandler>> _mockLogger;
        private GetAuthorByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            // Mocka IRepository och Logger
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockLogger = new Mock<ILogger<GetAuthorByIdQueryHandler>>();

            // Skapa instans av handlern
            _handler = new GetAuthorByIdQueryHandler(_mockAuthorRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnAuthor_WhenAuthorExists()
        {
            // Arrange
            var authorId = 1;
            var author = new Author { Id = authorId, Name = "Author One" };

            // Mocka GetAuthorByIdAsync för att returnera en lyckad OperationResult
            _mockAuthorRepository
                .Setup(repo => repo.GetAuthorById(authorId))
                .ReturnsAsync(OperationResult<Author>.Success(author));

            // Act
            var result = await _handler.Handle(new GetAuthorByIdQuery(authorId), CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.Id, Is.EqualTo(authorId));
            Assert.That(result.Data.Name, Is.EqualTo("Author One")); 
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = 999; // Ett ID som inte finns i databasen

            // Mocka GetAuthorByIdAsync för att returnera ett misslyckat OperationResult
            _mockAuthorRepository
                .Setup(repo => repo.GetAuthorById(authorId))
                .ReturnsAsync(OperationResult<Author>.Failure("Author not found."));

            // Act
            var result = await _handler.Handle(new GetAuthorByIdQuery(authorId), CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False); // Kontrollera att resultatet är ett misslyckande
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found.")); // Kontrollera att felmeddelandet är rätt
        }
    }
}
