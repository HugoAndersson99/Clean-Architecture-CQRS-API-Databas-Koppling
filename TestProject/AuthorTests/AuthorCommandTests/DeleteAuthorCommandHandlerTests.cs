using Application.Commands.Authors.DeleteAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;


namespace TestProject.AuthorTests.AuthorCommandTests
{
    [TestFixture]
    public class DeleteAuthorCommandHandlerTests
    {
        private DeleteAuthorCommandHandler _handler;
        private IAuthorRepository _mockAuthorRepository;
        private ILogger<DeleteAuthorCommandHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockAuthorRepository = A.Fake<IAuthorRepository>();

            _mockLogger = A.Fake<ILogger<DeleteAuthorCommandHandler>>();

            _handler = new DeleteAuthorCommandHandler(_mockAuthorRepository, _mockLogger);
        }
        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorIsDeleted()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Test Author" };
            var command = new DeleteAuthorCommand(1);

            A.CallTo(() => _mockAuthorRepository.GetAuthorById(1))
                .Returns(Task.FromResult(OperationResult<Author>.Success(author, "Author found.")));

            A.CallTo(() => _mockAuthorRepository.DeleteAuthorById(1))
                .Returns(Task.FromResult(OperationResult<string>.Success("Author deleted successfully.", "Delete operation successful.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Author deleted successfully.", result.Data);
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorDoesNotExist()
        {
            // Arrange
            var command = new DeleteAuthorCommand(1);

            A.CallTo(() => _mockAuthorRepository.DeleteAuthorById(1))
                .Returns(Task.FromResult(OperationResult<string>.Failure("Author with Id 1 not found.", "Entity not found.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Author with Id 1 not found.", result.ErrorMessage);
        }
        
    }
}
