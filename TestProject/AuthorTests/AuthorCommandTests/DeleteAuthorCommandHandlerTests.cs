using Application.Commands.Authors.DeleteAuthor;
using Domain;
using Infrastructure.Database;

namespace TestProject.AuthorTests.AuthorCommandTests
{
    [TestFixture]
    public class DeleteAuthorCommandHandlerTests
    {
        private FakeDatabase _database;
        private DeleteAuthorCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _database = new FakeDatabase();
            _handler = new DeleteAuthorCommandHandler(_database);
        }

        [Test]
        public async Task Handle_ShouldDeleteAuthor_WhenAuthorExists()
        {
            // Arrange
            int IdOfExistingAuthorToDelete = 1;
            var command = new DeleteAuthorCommand(IdOfExistingAuthorToDelete);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Any(a => a.Id == 1), Is.False, "Author with Id 1 should be deleted.");
        }

        [Test]
        public void Handle_ShouldThrowArgumentException_WhenIdIsInvalid()
        {
            // Arrange
            int invalidId = 0;
            var command = new DeleteAuthorCommand(invalidId);

            // Act & Assert
            var exceptionMessageReturn = Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exceptionMessageReturn.Message, Does.Contain("Id must be greater than 0."), "Exception message should match.");
        }

        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            int idOfNoExistingAuthor = 999;
            var command = new DeleteAuthorCommand(idOfNoExistingAuthor);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Does.Contain($"No author found with Id {idOfNoExistingAuthor}."), "Exception message should mention the missing author.");
        }

        [Test]
        public async Task Handle_ShouldHandleEmptyAuthorList()
        {
            // Arrange
            int authorId = 1;
            _database.Authors.Clear(); 
            var command = new DeleteAuthorCommand(authorId);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Does.Contain($"No author found with Id {authorId}."), "Exception message should match.");
        }
    }
}
