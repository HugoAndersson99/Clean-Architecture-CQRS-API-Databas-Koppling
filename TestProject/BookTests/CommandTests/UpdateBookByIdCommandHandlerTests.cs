using Application.Commands.Books.UpdateBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.CommandTests
{
   // [TestFixture]
   // public class UpdateBookByIdCommandHandlerTests
   // {
   //     private Mock<IBookRepository> _bookRepositoryMock;
   //     private Mock<ILogger<UpdateBookByIdCommandHandler>> _loggerMock;
   //     private UpdateBookByIdCommandHandler _handler;
   //
   //     [SetUp]
   //     public void Setup()
   //     {
   //         _bookRepositoryMock = new Mock<IBookRepository>();
   //         _loggerMock = new Mock<ILogger<UpdateBookByIdCommandHandler>>();
   //         _handler = new UpdateBookByIdCommandHandler(_bookRepositoryMock.Object, _loggerMock.Object);
   //     }
   //
   //     [Test]
   //     public async Task Handle_ShouldUpdateBook_WhenBookExists()
   //     {
   //         // Arrange
   //         var bookId = 1;
   //         var updatedBook = new Book
   //         {
   //             Title = "Updated Book Title",
   //             Description = "Updated Book Description"
   //         };
   //         var command = new UpdateBookByIdCommand(bookId, updatedBook);
   //
   //         _bookRepositoryMock
   //             .Setup(repo => repo.UpdateBook(It.IsAny<int>(), It.IsAny<Book>()))
   //             .ReturnsAsync(OperationResult<Book>.Success(updatedBook, "Book updated successfully."));
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.IsTrue(result.IsSuccess);
   //         Assert.AreEqual("Book updated successfully.", result.Message);
   //         _bookRepositoryMock.Verify(repo => repo.UpdateBook(It.IsAny<int>(), It.IsAny<Book>()), Times.Once);
   //     }
   //
   //     [Test]
   //     public async Task Handle_ShouldReturnFailure_WhenBookDoesNotExist()
   //     {
   //         // Arrange
   //         var bookId = 999; // Assume this ID does not exist
   //         var updatedBook = new Book
   //         {
   //             Title = "Updated Book Title",
   //             Description = "Updated Book Description"
   //         };
   //         var command = new UpdateBookByIdCommand(bookId, updatedBook);
   //
   //         _bookRepositoryMock
   //             .Setup(repo => repo.UpdateBook(It.IsAny<int>(), It.IsAny<Book>()))
   //             .ReturnsAsync(OperationResult<Book>.Failure("Book not found.", "Database error."));
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.IsFalse(result.IsSuccess);
   //         Assert.AreEqual("Book not found.", result.Message);
   //     }
   //
   //     [Test]
   //     public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
   //     {
   //         // Arrange
   //         var bookId = 1;
   //         var updatedBook = new Book
   //         {
   //             Title = "Updated Book Title",
   //             Description = "Updated Book Description"
   //         };
   //         var command = new UpdateBookByIdCommand(bookId, updatedBook);
   //
   //         _bookRepositoryMock
   //             .Setup(repo => repo.UpdateBook(It.IsAny<int>(), It.IsAny<Book>()))
   //             .ReturnsAsync(OperationResult<Book>.Failure("An error occurred while updating the book."));
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.IsFalse(result.IsSuccess);
   //         Assert.AreEqual("An error occurred while updating the book.", result.Message);
   //     }
   // }
}
