using Application.Queries.Authors.GetAllAuthors;
using Infrastructure.Database;

namespace TestProject.AuthorTests.AuthorQueryTests
{
    [TestFixture]
    public class GetAllAuthorsQueryHandlerTests
    {
        private FakeDatabase _database;
        private GetAllAuthorsQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _database = new FakeDatabase();
            _handler = new GetAllAuthorsQueryHandler(_database);
        }
        [Test]
        public async Task Handle_ShouldReturnAllAuthors_WhenAuthorsExist()
        {
            // Arrange
            var query = new GetAllAuthorsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Count, Is.EqualTo(_database.Authors.Count), "The number of authors should match the database count.");
            CollectionAssert.AreEquivalent(_database.Authors, result, "The books returned should match the books in the database.");
        }
        [Test]
        public void Handle_ShouldThrowInvalidOperationException_WhenNoAuthorsExist()
        {
            // Arrange
            _database.Authors.Clear(); // Töm databasen
            var query = new GetAllAuthorsQuery();

            // Act & Assert
            var exceptionMessage = Assert.ThrowsAsync<InvalidOperationException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(exceptionMessage.Message, Does.Contain("No authors found in the database."), "Exception message should match.");
        }
    }
}
