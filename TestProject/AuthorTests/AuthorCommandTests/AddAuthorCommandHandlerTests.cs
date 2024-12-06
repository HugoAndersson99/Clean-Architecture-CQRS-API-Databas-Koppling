using Application.Commands.Authors.AddAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.AuthorTests.AuthorCommandTests
{
   //[TestFixture]
   //public class AddAuthorCommandHandlerTests
   //{
   //    private Mock<IAuthorRepository> _authorRepositoryMock;
   //    private Mock<ILogger<AddAuthorCommandHandler>> _loggerMock;
   //    private AddAuthorCommandHandler _handler;
   //
   //    [SetUp]
   //    public void Setup()
   //    {
   //        _authorRepositoryMock = new Mock<IAuthorRepository>();
   //        _loggerMock = new Mock<ILogger<AddAuthorCommandHandler>>();
   //        _handler = new AddAuthorCommandHandler(_authorRepositoryMock.Object, _loggerMock.Object);
   //    }
   //
   //    [Test]
   //    public async Task Handle_ShouldReturnSuccess_WhenAuthorIsAdded()
   //    {
   //        // Arrange
   //        Author newAuthor = new Author(10, "HugoTest");
   //        var command = new AddAuthorCommand(newAuthor);
   //
   //        _authorRepositoryMock
   //            .Setup(repo => repo.AddAuthor(newAuthor))
   //            .ReturnsAsync(OperationResult<Author>.Success(newAuthor, "Author added successfully."));
   //
   //        // Act
   //        var result = await _handler.Handle(command, CancellationToken.None);
   //
   //        // Assert
   //        Assert.That(result.IsSuccess, Is.True, "The operation should be successful.");
   //        Assert.That(result.Data, Is.Not.Null, "The result data should not be null.");
   //        Assert.That(result.Data.Name, Is.EqualTo(newAuthor.Name), "The returned author name should match the input.");
   //        _authorRepositoryMock.Verify(repo => repo.AddAuthor(newAuthor), Times.Once);
   //        _loggerMock.Verify(
   //            logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()),
   //            Times.AtLeastOnce
   //        );
   //    }
   //
   //    [Test]
   //    public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
   //    {
   //        // Arrange
   //        var newAuthor = new Author { Name = "Test Author" };
   //        var command = new AddAuthorCommand(newAuthor);
   //
   //        _authorRepositoryMock
   //            .Setup(repo => repo.AddAuthor(newAuthor))
   //            .ReturnsAsync(OperationResult<Author>.Failure("Failed to add author.", "Database error."));
   //
   //        // Act
   //        var result = await _handler.Handle(command, CancellationToken.None);
   //
   //        // Assert
   //        Assert.That(result.IsSuccess, Is.False, "The operation should fail.");
   //        Assert.That(result.ErrorMessage, Is.EqualTo("Failed to add author."), "The error message should match.");
   //        _authorRepositoryMock.Verify(repo => repo.AddAuthor(newAuthor), Times.Once);
   //        _loggerMock.Verify(
   //            logger => logger.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()),
   //            Times.AtLeastOnce
   //        );
   //    }
   //
   //    [Test]
   //    public void Handle_ShouldThrowArgumentNullException_WhenAuthorIsNull()
   //    {
   //        // Arrange
   //        var command = new AddAuthorCommand(null);
   //
   //        // Act & Assert
   //        Assert.ThrowsAsync<ArgumentNullException>(async () => await _handler.Handle(command, CancellationToken.None));
   //        _authorRepositoryMock.Verify(repo => repo.AddAuthor(It.IsAny<Author>()), Times.Never);
   //        _loggerMock.Verify(
   //            logger => logger.LogError(It.IsAny<ArgumentNullException>(), It.IsAny<string>(), It.IsAny<object[]>()),
   //            Times.Once
   //        );
   //    }
   //
   //    [Test]
   //    public async Task Handle_ShouldReturnFailure_WhenAuthorNameIsEmpty()
   //    {
   //        // Arrange
   //        var newAuthor = new Author { Name = "" };
   //        var command = new AddAuthorCommand(newAuthor);
   //
   //        // Act & Assert
   //        var result = await _handler.Handle(command, CancellationToken.None);
   //
   //        Assert.That(result.IsSuccess, Is.False, "The operation should fail.");
   //        Assert.That(result.ErrorMessage, Is.EqualTo("Author name must not be empty."), "The error message should match.");
   //        _authorRepositoryMock.Verify(repo => repo.AddAuthor(It.IsAny<Author>()), Times.Never);
   //        _loggerMock.Verify(
   //            logger => logger.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()),
   //            Times.AtLeastOnce
   //        );
   //    }
   //}
}
