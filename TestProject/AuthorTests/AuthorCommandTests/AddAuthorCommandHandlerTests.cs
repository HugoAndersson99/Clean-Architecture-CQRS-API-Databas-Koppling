using Application.ApplicationDtos;
using Application.Commands.Authors.AddAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace TestProject.AuthorTests.AuthorCommandTests
{
   [TestFixture]
   public class AddAuthorCommandHandlerTests
   {
        private AddAuthorCommandHandler _handler;
        private IAuthorRepository _mockAuthorRepository;
        private ILogger<AddAuthorCommandHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mocka repositoryn
            _mockAuthorRepository = A.Fake<IAuthorRepository>();

            _mockLogger = A.Fake<ILogger<AddAuthorCommandHandler>>();

            // Skapa handlern med mockad repository
            _handler = new AddAuthorCommandHandler(_mockAuthorRepository, _mockLogger);
        }
        [Test]
        public async Task Handle_ShouldAddAuthorSuccessfully_WhenRepositoryReturnsSuccess()
        {
            // Arrange
            // Skapa en AuthorDto för att skicka med i kommandot
            var authorDto = new AuthorDto { Name = "Hugo" };
            var command = new AddAuthorCommand(authorDto);  // Skicka DTO i kommandot

            // Konvertera AuthorDto till Author innan vi skickar det till repositoryn
            var author = new Author { Name = authorDto.Name };  // Skapa ett domänobjekt från DTO

            // Mocka resultatet från repositoryn
            A.CallTo(() => _mockAuthorRepository.AddAuthor(A<AuthorDto>.Ignored))
                .Returns(Task.FromResult(OperationResult<Author>.Success(author, "Author added successfully.")));

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Hugo", result.Data.Name);
            A.CallTo(() => _mockAuthorRepository.AddAuthor(A<AuthorDto>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryFailsToAddAuthor()
        {
            // Arrange
            var authorDto = new AuthorDto { Name = "Hugo" };
            var command = new AddAuthorCommand(authorDto);  // Skicka DTO i kommandot

            // Mocka ett misslyckande från repositoryn
            A.CallTo(() => _mockAuthorRepository.AddAuthor(A<AuthorDto>.Ignored))
                .Returns(Task.FromResult(OperationResult<Author>.Failure("Failed to add author.")));

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to add author.", result.ErrorMessage);
            A.CallTo(() => _mockAuthorRepository.AddAuthor(A<AuthorDto>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorHasInvalidData()
        {
            // Arrange
            var invalidAuthor = new AuthorDto { Name = "" }; // Tomt namn
            var command = new AddAuthorCommand(invalidAuthor);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Author name cannot be empty", result.ErrorMessage); // Kontrollera felmeddelandet
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryReturnsNull()
        {
            // Arrange
            var authorToAdd = new AuthorDto { Name = "New Author" };
            var command = new AddAuthorCommand(authorToAdd);

            // Mocka repository-anropet och konfigurera det att returnera ett failure-svar
            A.CallTo(() => _mockAuthorRepository.AddAuthor(A<AuthorDto>.Ignored))
                .Returns(Task.FromResult(OperationResult<Author>.Failure("Failed to add author")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to add author.", result.ErrorMessage);
        }
        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorIsAddedSuccessfully()
        {
            // Arrange
            var authorToAdd = new AuthorDto { Name = "Valid Author" };
            var command = new AddAuthorCommand(authorToAdd);

            var expectedAuthor = new Author { Id = 1, Name = "Valid Author" };
            A.CallTo(() => _mockAuthorRepository.AddAuthor(A<AuthorDto>.Ignored))
                .Returns(Task.FromResult(OperationResult<Author>.Success(expectedAuthor, "Author added successfully")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Valid Author", result.Data.Name);
            Assert.AreEqual(1, result.Data.Id);
        }
    }
}
