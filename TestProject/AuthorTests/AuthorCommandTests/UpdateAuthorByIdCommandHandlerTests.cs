using Application.Commands.Authors.UpdateAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.AuthorTests.AuthorCommandTests
{
  //  [TestFixture]
  //  public class UpdateAuthorByIdCommandHandlerTests
  //  {
  //      private Mock<IAuthorRepository> _mockAuthorRepository;
  //      private Mock<ILogger<UpdateAuthorByIdCommandHandler>> _mockLogger;
  //      private UpdateAuthorByIdCommandHandler _handler;
  //
  //      [SetUp]
  //      public void SetUp()
  //      {
  //          // Mocka repository och logger
  //          _mockAuthorRepository = new Mock<IAuthorRepository>();
  //          _mockLogger = new Mock<ILogger<UpdateAuthorByIdCommandHandler>>();
  //
  //          // Skapa handlern med mockade beroenden
  //          _handler = new UpdateAuthorByIdCommandHandler(_mockAuthorRepository.Object, _mockLogger.Object);
  //      }
  //
  //      [Test]
  //      public async Task Handle_ShouldReturnSuccess_WhenAuthorUpdated()
  //      {
  //          // Arrange
  //          var authorToUpdate = new Author { Id = 1, Name = "Updated Author" };
  //          var command = new UpdateAuthorByIdCommand(authorToUpdate, 1);
  //
  //          // Mocka att repositoryn returnerar ett lyckat resultat
  //          _mockAuthorRepository
  //              .Setup(repo => repo.UpdateAuthor(authorToUpdate, 1))
  //              .ReturnsAsync(OperationResult<Author>.Success(authorToUpdate, "Author updated successfully."));
  //
  //          // Act
  //          var result = await _handler.Handle(command, CancellationToken.None);
  //
  //          // Assert
  //          Assert.That(result.IsSuccess, Is.True);
  //          Assert.That(result.Data.Name, Is.EqualTo("Updated Author"));
  //          _mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
  //      }
  //
  //      [Test]
  //      public async Task Handle_ShouldReturnFailure_WhenAuthorNotFound()
  //      {
  //          // Arrange
  //          var authorToUpdate = new Author { Id = 1, Name = "Updated Author" };
  //          var command = new UpdateAuthorByIdCommand(authorToUpdate, 1);
  //
  //          // Mocka att repositoryn inte hittar författaren
  //          _mockAuthorRepository
  //              .Setup(repo => repo.UpdateAuthor(authorToUpdate, 1))
  //              .ReturnsAsync(OperationResult<Author>.Failure("Author not found", "Update failed"));
  //
  //          // Act
  //          var result = await _handler.Handle(command, CancellationToken.None);
  //
  //          // Assert
  //          Assert.That(result.IsSuccess, Is.False);
  //          Assert.That(result.ErrorMessage, Is.EqualTo("Author not found"));
  //          _mockLogger.Verify(logger => logger.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
  //      }
  //
  //      [Test]
  //      public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
  //      {
  //          // Arrange
  //          var authorToUpdate = new Author { Id = 1, Name = "Updated Author" };
  //          var command = new UpdateAuthorByIdCommand(authorToUpdate, 1);
  //
  //          // Mocka att repositoryn kastar ett undantag
  //          _mockAuthorRepository
  //              .Setup(repo => repo.UpdateAuthor(authorToUpdate, 1))
  //              .ThrowsAsync(new Exception("Database error"));
  //
  //          // Act
  //          var result = await _handler.Handle(command, CancellationToken.None);
  //
  //          // Assert
  //          Assert.That(result.IsSuccess, Is.False);
  //          Assert.That(result.ErrorMessage, Is.EqualTo("An error occurred: Database error"));
  //          _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
  //      }
  //  }
}
