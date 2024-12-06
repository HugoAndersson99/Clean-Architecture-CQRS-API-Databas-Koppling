﻿using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetAll;
using Application.Queries.Books.GetById;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.QueryTests
{
    [TestFixture]
    public class GetBookByIdQueryHandlerTests
    {
        private GetBookByIdQueryHandler _handler;
        private IBookRepository _mockBookRepository;
        private ILogger<GetBookByIdQueryHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            // Mocka repository
            _mockBookRepository = A.Fake<IBookRepository>();

            // Mocka logger
            _mockLogger = A.Fake<ILogger<GetBookByIdQueryHandler>>();

            // Skapa handlern med mockad repository
            _handler = new GetBookByIdQueryHandler(_mockBookRepository, _mockLogger);
        }
        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenBookIsFound()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book", Description = "Test Description" };

            // Simulera ett lyckat resultat från GetBookById
            A.CallTo(() => _mockBookRepository.GetBookById(1))
                .Returns(OperationResult<Book>.Success(book, "Book retrieved successfully."));

            // Act
            var result = await _handler.Handle(new GetBookByIdQuery(1), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Test Book", result.Data.Title);
            Assert.AreEqual("Test Description", result.Data.Description);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookNotFound()
        {
            // Arrange
            // Simulera att boken inte hittas i repository
            A.CallTo(() => _mockBookRepository.GetBookById(1))
                .Returns(OperationResult<Book>.Failure("Book not found.", "Database error."));

            // Act
            var result = await _handler.Handle(new GetBookByIdQuery(1), CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Book not found.", result.ErrorMessage);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            // Simulera att ett undantag inträffar i repository
            A.CallTo(() => _mockBookRepository.GetBookById(1))
                .Throws(new Exception("Database failure"));

            // Act
            var result = await _handler.Handle(new GetBookByIdQuery(1), CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("An error occurred: Database failure", result.ErrorMessage);
        }
    }
}
