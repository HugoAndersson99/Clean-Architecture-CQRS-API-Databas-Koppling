﻿using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetAll;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace TestProject.BookTests.QueryTests
{
    [TestFixture]
    public class GetAllBooksQueryHandlerTests
    {
        private GetAllBooksQueryHandler _handler;
        private IBookRepository _mockBookRepository;
        private ILogger<GetAllBooksQueryHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockBookRepository = A.Fake<IBookRepository>();

            _mockLogger = A.Fake<ILogger<GetAllBooksQueryHandler>>();

            var _mockCache = A.Fake<IMemoryCache>();

            _handler = new GetAllBooksQueryHandler(_mockBookRepository, _mockLogger, _mockCache);
        }

        [Test]
        public async Task Handle_ShouldReturnBooks_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Description = "Description 1" },
            new Book { Id = 2, Title = "Book 2", Description = "Description 2" }
        };

            var command = new GetAllBooksQuery();

            A.CallTo(() => _mockBookRepository.GetAllBooks())
                .Returns(Task.FromResult(OperationResult<List<Book>>.Success(books, "Books retrieved successfully.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual("Books retrieved successfully.", result.Message);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenNoBooksExist()
        {
            // Arrange
            var command = new GetAllBooksQuery();

            A.CallTo(() => _mockBookRepository.GetAllBooks())
                .Returns(Task.FromResult(OperationResult<List<Book>>.Failure("No books found.", "Database error.")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("No books found.", result.ErrorMessage);
        }

        
    }
}
