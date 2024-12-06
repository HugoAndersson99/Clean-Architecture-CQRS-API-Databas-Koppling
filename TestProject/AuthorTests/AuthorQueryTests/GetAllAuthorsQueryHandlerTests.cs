using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAllAuthors;
using Domain;
using Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.AuthorTests.AuthorQueryTests
{
    [TestFixture]
    public class GetAllAuthorsQueryHandlerTests
    {
        private Mock<IAuthorRepository> _mockAuthorRepository;
        private Mock<ILogger<GetAllAuthorsQueryHandler>> _mockLogger;
        private GetAllAuthorsQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockLogger = new Mock<ILogger<GetAllAuthorsQueryHandler>>();
            _handler = new GetAllAuthorsQueryHandler(_mockAuthorRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnAuthors_WhenAuthorsExist()
        {
            // Arrange
            var authors = new List<Author>
        {
            new Author { Id = 1, Name = "Author One" },
            new Author { Id = 2, Name = "Author Two" }
        };

            // Mocka GetAllAuthorsAsync för att returnera en lyckad OperationResult
            _mockAuthorRepository
                .Setup(repo => repo.GetAllAuthors())
                .ReturnsAsync(OperationResult<List<Author>>.Success(authors));

            // Act
            var result = await _handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True); // Kontrollera att resultatet är en framgång
            Assert.That(result.Data.Count, Is.EqualTo(2)); // Kontrollera att listan innehåller 2 författare
            Assert.That(result.Data[0].Name, Is.EqualTo("Author One")); // Kontrollera att namnet på den första författaren är rätt
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenNoAuthorsExist()
        {
            // Arrange
            // Mocka GetAllAuthorsAsync för att returnera ett misslyckat OperationResult
            _mockAuthorRepository
                .Setup(repo => repo.GetAllAuthors())
                .ReturnsAsync(OperationResult<List<Author>>.Failure("No authors found."));

            // Act
            var result = await _handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False); // Kontrollera att resultatet är ett misslyckande
            Assert.That(result.ErrorMessage, Is.EqualTo("No authors found.")); // Kontrollera felmeddelandet
        }
    }
}
