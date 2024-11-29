using Application.Commands.Authors.AddAuthor;
using Domain;
using Infrastructure.Database;

namespace TestProject.AuthorTests.AuthorCommandTests
{
    [TestFixture]
    public class AddAuthorCommandHandlerTests
    {
        private FakeDatabase _database;
        private AddAuthorCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _database = new FakeDatabase();
            _handler = new AddAuthorCommandHandler(_database);
        }

        [Test]
        public async Task Handle_ShouldAddAuthor_WhenValidAuthorIsProvided()
        {
            // Arrange
            var newAuthor = new Author { Name = "New Author" };
            var command = new AddAuthorCommand(newAuthor);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result, Has.Exactly(1).Matches<Author>(a => a.Name == "New Author"), "Author should be added.");
        }

        [Test]
        public void Handle_ShouldThrowArgumentNullException_WhenAuthorIsNull()
        {
            // Arrange
            var command = new AddAuthorCommand(null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _handler.Handle(command, CancellationToken.None));
            
        }

        [Test]
        public void Handle_ShouldThrowArgumentException_WhenAuthorNameIsEmpty()
        {
            // Arrange
            var newAuthor = new Author { Name = "" }; // Empty name
            var command = new AddAuthorCommand(newAuthor);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.ParamName, Is.EqualTo("Name"), "Exception should mention the parameter name.");
            Assert.That(ex.Message, Does.Contain("Author name must not be empty."), "Exception message should match.");
        }

        [Test]
        public async Task Handle_ShouldAssignNewId_WhenAuthorIsAdded()
        {
            // Arrange
            var newAuthor = new Author { Name = "Unique Author" };
            var command = new AddAuthorCommand(newAuthor);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            var addedAuthor = result.FirstOrDefault(a => a.Name == "Unique Author");

            // Assert
            Assert.That(addedAuthor, Is.Not.Null, "Author should be added to the database.");
            Assert.That(addedAuthor.Id, Is.GreaterThan(0), "New author should have a valid ID.");
        }

        [Test]
        public async Task Handle_ShouldIncrementId_WhenMultipleAuthorsAreAdded()
        {
            // Arrange
            var firstAuthor = new Author { Name = "First Author" };
            var secondAuthor = new Author { Name = "Second Author" };
            await _handler.Handle(new AddAuthorCommand(firstAuthor), CancellationToken.None);

            // Act
            var result = await _handler.Handle(new AddAuthorCommand(secondAuthor), CancellationToken.None);
            var secondAddedAuthor = result.FirstOrDefault(a => a.Name == "Second Author");

            // Assert
            Assert.That(secondAddedAuthor, Is.Not.Null, "Second author should be added to the database.");
            Assert.That(secondAddedAuthor.Id, Is.EqualTo(firstAuthor.Id + 1), "ID of the second author should be incremented.");
        }
    }
}
