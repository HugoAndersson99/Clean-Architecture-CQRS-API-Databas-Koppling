using Application.Commands.Authors.DeleteAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.AuthorTests.AuthorCommandTests
{
    [TestFixture]
    public class DeleteAuthorCommandHandlerTests
    {
        private Mock<IAuthorRepository> _authorRepositoryMock;
        private Mock<ILogger<DeleteAuthorCommandHandler>> _loggerMock;
        private DeleteAuthorCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _loggerMock = new Mock<ILogger<DeleteAuthorCommandHandler>>();
            _handler = new DeleteAuthorCommandHandler(_authorRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorIsDeleted()
        {
            // Arrange
            var authorId = 1;
            _authorRepositoryMock
                .Setup(repo => repo.DeleteAuthorById(authorId))
                .ReturnsAsync(OperationResult<string>.Success("Author deleted successfully.", "Delete operation successful."));

            var command = new DeleteAuthorCommand(authorId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.EqualTo("Author deleted successfully."));
            _authorRepositoryMock.Verify(repo => repo.DeleteAuthorById(authorId), Times.Once);
            _loggerMock.Verify(
                logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()),
                Times.AtLeastOnce
            );
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorNotFound()
        {
            // Arrange
            var authorId = 99;
            _authorRepositoryMock
                .Setup(repo => repo.DeleteAuthorById(authorId))
                .ReturnsAsync(OperationResult<string>.Failure("Author with Id 99 not found.", "Entity not found."));

            var command = new DeleteAuthorCommand(authorId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author with Id 99 not found."));
            _authorRepositoryMock.Verify(repo => repo.DeleteAuthorById(authorId), Times.Once);
            _loggerMock.Verify(
                logger => logger.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()),
                Times.AtLeastOnce
            );
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            // Arrange
            var authorId = 1;
            _authorRepositoryMock
                .Setup(repo => repo.DeleteAuthorById(authorId))
                .ThrowsAsync(new Exception("Database error"));

            var command = new DeleteAuthorCommand(authorId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("An unexpected error occurred."));
            _authorRepositoryMock.Verify(repo => repo.DeleteAuthorById(authorId), Times.Once);
            _loggerMock.Verify(
                logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()),
                Times.Once
            );
        }
    }
}
