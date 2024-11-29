using Application.Queries.Authors.GetAuthorById;
using Domain;
using Infrastructure.Database;

namespace TestProject.AuthorTests.AuthorQueryTests
{
    [TestFixture]
    public class GetAuthorByIdQueryHandlerTests
    {
        private FakeDatabase _database;
        private GetAuthorByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _database = new FakeDatabase();
            _handler = new GetAuthorByIdQueryHandler(_database);
        }

        [Test]
        public async Task Handle_ShouldReturnAuthor_WhenAuthorExists()
        {
            // Arrange
            int existingIdOfAuhtor = 1;
            Author choosenAuthor = _database.Authors.FirstOrDefault(author => author.Id == existingIdOfAuhtor);
            
            var query = new GetAuthorByIdQuery(existingIdOfAuhtor);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Id, Is.EqualTo(existingIdOfAuhtor), "Author ID should match the requested ID.");
            Assert.That(result.Name, Is.EqualTo(choosenAuthor.Name), "Author name should match the expected name.");
        }

        [Test]
        public void Handle_ShouldThrowArgumentException_WhenIdIsInvalid()
        {
            // Arrange
            int invalidId = 0;
            var query = new GetAuthorByIdQuery(invalidId);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(ex.Message, Does.Contain("Id must be greater than 0"), "Exception message should match.");
        }
        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            int nonExistingId = 999;
            var query = new GetAuthorByIdQuery(nonExistingId);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(ex.Message, Does.Contain($"Author with Id {nonExistingId} not found"), "Exception message should match.");
        }
        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenDatabaseIsEmpty()
        {
            // Arrange
            int authorId = 1;
            _database.Authors.Clear(); // Töm databasen
            var query = new GetAuthorByIdQuery(authorId);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(ex.Message, Does.Contain($"Author with Id {authorId} not found"), "Exception message should match.");
        }
        [Test]
        public async Task Handle_ShouldReturnAuthorWithBooks_WhenAuthorExists()
        {
            // Arrange
            var query = new GetAuthorByIdQuery(1); // Författare med ID 1 ("Hugo") finns i databasen

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Id, Is.EqualTo(1), "Author ID should match the requested ID.");
            Assert.That(result.Name, Is.EqualTo("Hugo"), "Author name should match the expected name.");
            Assert.That(result.Books, Is.Not.Null, "Author's book list should not be null.");
            Assert.That(result.Books.Count, Is.EqualTo(2), "Author's book list should contain the expected number of books.");

            // Kontrollera att rätt böcker returneras
            Assert.That(result.Books[0].Title, Is.EqualTo("FirstBookOfHugo"), "The first book title should match.");
            Assert.That(result.Books[1].Title, Is.EqualTo("SecondBookOfHugo"), "The second book title should match.");
        }
    }
}
