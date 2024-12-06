using Application.ApplicationDtos;
using Application.Commands.Authors.DeleteAuthor;
using Application.Commands.Authors.UpdateAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.AuthorTests.AuthorCommandTests
{
    [TestFixture]
    public class UpdateAuthorByIdCommandHandlerTests
    {
        private UpdateAuthorByIdCommandHandler _handler;
        private IAuthorRepository _mockAuthorRepository;
        private ILogger<UpdateAuthorByIdCommandHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mocka repositoryn
            _mockAuthorRepository = A.Fake<IAuthorRepository>();

            _mockLogger = A.Fake<ILogger<UpdateAuthorByIdCommandHandler>>();

            // Skapa handlern med mockad repository
            _handler = new UpdateAuthorByIdCommandHandler(_mockAuthorRepository, _mockLogger);
        }
        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorIsUpdated()
        {
            // Arrange
            var command = new UpdateAuthorByIdCommand(new AuthorDto { Name = "Updated Author Name" }, 1);

            // Mocka repository så att den returnerar en författare med det uppdaterade namnet
            var updatedAuthor = new Author { Id = 1, Name = "Updated Author Name" }; // Skapa den uppdaterade författaren

            // Mocka att UpdateAuthor anropas med korrekt AuthorDto och id och returnera den uppdaterade författaren
            A.CallTo(() => _mockAuthorRepository.UpdateAuthor(A<AuthorDto>.That.Matches(a => a.Name == "Updated Author Name"), 1))
                .Returns(Task.FromResult(OperationResult<Author>.Success(updatedAuthor, "Author updated successfully.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Author updated successfully.", result.Message);
            Assert.AreEqual("Updated Author Name", result.Data.Name);  // Förväntad namnändring
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorDoesNotExist()
        {
            // Arrange
            var command = new UpdateAuthorByIdCommand(new AuthorDto { Name = "Non-existent Author" }, 99);

            // Mocka repository så att den returnerar att författaren inte finns
            A.CallTo(() => _mockAuthorRepository.UpdateAuthor(A<AuthorDto>.That.Matches(a => a.Name == "Non-existent Author"), 99))
                .Returns(Task.FromResult(OperationResult<Author>.Failure("Author not found", "Update failed")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to update author: Author not found", result.ErrorMessage);
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorDataIsInvalid()
        {
            // Arrange
            var command = new UpdateAuthorByIdCommand(new AuthorDto { Name = string.Empty }, 1);

            // Mocka repository så att den inte försöker uppdatera eftersom data är ogiltig
            A.CallTo(() => _mockAuthorRepository.UpdateAuthor(A<AuthorDto>.That.Matches(a => string.IsNullOrWhiteSpace(a.Name)), 1))
                .Returns(Task.FromResult(OperationResult<Author>.Failure("Invalid author data", "Invalid Id.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to update author: Invalid author data", result.ErrorMessage);
        }
        
    }
}
