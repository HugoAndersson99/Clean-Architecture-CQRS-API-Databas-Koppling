using Application.Queries.Books.GetAll;
using Infrastructure.Database;

namespace TestProject.BookTests.QueryTests
{
    [TestFixture]
    public class GetAllBooksQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private GetAllBooksQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new GetAllBooksQueryHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldReturnAllBooks_WhenBooksExist()
        {
            // Arrange
            var query = new GetAllBooksQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Count, Is.EqualTo(_fakeDatabase.Books.Count), "The number of books returned should match the number of books in the database.");
            CollectionAssert.AreEquivalent(_fakeDatabase.Books, result, "The books returned should match the books in the database.");
        }

        [Test]
        public void Handle_ShouldThrowInvalidOperationException_WhenNoBooksExist()
        {
            // Arrange
            _fakeDatabase.Books.Clear(); // Clear all books from the database
            var query = new GetAllBooksQuery();

            // Act & Assert
            var exceptionMessageReturn = Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.That(exceptionMessageReturn.Message, Is.EqualTo("No books found in the database."));
        }
    }
}
