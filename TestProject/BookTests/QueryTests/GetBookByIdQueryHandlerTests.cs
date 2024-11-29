using Application.Queries.Books.GetById;
using Domain;
using Infrastructure.Database;

namespace TestProject.BookTests.QueryTests
{
    [TestFixture]
    public class GetBookByIdQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private GetBookByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new GetBookByIdQueryHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            Book existingBook = _fakeDatabase.Books.First();
            var query = new GetBookByIdQuery(existingBook.Id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Id, Is.EqualTo(existingBook.Id), "The book ID should match the one in the query.");
            Assert.That(result.Title, Is.EqualTo(existingBook.Title), "The book title should match the expected title.");
        }

        [Test]
        public void Handle_ShouldThrowArgumentException_WhenIdIsInvalid()
        {
            // Arrange
            int invalidId = -1;
            var query = new GetBookByIdQuery(invalidId);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Id must be greater than 0. (Parameter 'Id')"));
        }

        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenBookNotFound()
        {
            // Arrange
            int nonExistentId = 999;
            var query = new GetBookByIdQuery(nonExistentId); // Använd ett ID som inte finns i databasen

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo($"Book with Id {nonExistentId} not found."));
        }
    }
}
