using Application.Commands.Authors.UpdateAuthor;
using Domain;
using Infrastructure.Database;

namespace TestProject.AuthorTests.AuthorCommandTests
{
    [TestFixture]
    public class UpdateAuthorByIdCommandHandlerTests
    {
        private FakeDatabase _database;
        private UpdateAuthorByIdCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _database = new FakeDatabase();
            _handler = new UpdateAuthorByIdCommandHandler(_database);
        }

        [Test]
        public async Task Handle_ShouldUpdateAuthor_WhenValidDataProvided()
        {
            // Arrange
            int existingId = 1;
            var updatedAuthor = new Author(0, "Updated Author Name");
            var command = new UpdateAuthorByIdCommand(updatedAuthor, existingId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Id, Is.EqualTo(1), "The author ID should match the one in the query.");
            Assert.That(result.Name, Is.EqualTo("Updated Author Name"), "The author name should be updated.");
        }
        [Test]
        public void Handle_ShouldThrowArgumentNullException_WhenUpdatedAuthorIsNull()
        {
            // Arrange
            var command = new UpdateAuthorByIdCommand(null, 1); // Uppdaterad författare är null

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Does.Contain("Updated author details must be provided."), "Exception message should match.");
        }
        [Test]
        public void Handle_ShouldThrowArgumentException_WhenAuthorNameIsEmpty()
        {
            // Arrange
            var updatedAuthor = new Author(0, ""); // Namn är tomt
            var command = new UpdateAuthorByIdCommand(updatedAuthor, 1);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Does.Contain("Author name must not be empty."), "Exception message should match.");
        }
        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            int nonExistingId = 999;
            var updatedAuthor = new Author(0, "Nonexistent Author");
            var command = new UpdateAuthorByIdCommand(updatedAuthor, nonExistingId);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Does.Contain("Author with Id 999 not found."), "Exception message should mention the missing author.");
        }
        [Test]
        public async Task Handle_ShouldUpdateAuthorBooks_WhenBooksProvided()
        {
            // Arrange
            var updatedAuthor = new Author(0, "Author with Updated Books")
            {
                Books = new List<Book>
            {
                new Book(5, "New Book 1", "Genre 1"),
                new Book(6, "New Book 2", "Genre 2")
            }
            };
            var command = new UpdateAuthorByIdCommand(updatedAuthor, 1); // Författare med Id 1 finns i databasen

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Id, Is.EqualTo(1), "The author ID should match the one in the query.");
            Assert.That(result.Books.Count, Is.EqualTo(2), "The author should have the updated list of books.");
            Assert.That(result.Books[0].Title, Is.EqualTo("New Book 1"), "The first book title should match the updated book.");
            Assert.That(result.Books[1].Title, Is.EqualTo("New Book 2"), "The second book title should match the updated book.");
        }
    }
}
