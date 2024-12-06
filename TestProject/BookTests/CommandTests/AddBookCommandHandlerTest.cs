using Application.Commands.Books.AddBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.BookTests.CommandTests
{
   // [TestFixture]
   // public class AddBookCommandHandlerTest
   // {
   //     private Mock<IBookRepository> _bookRepositoryMock;
   //     private Mock<ILogger<AddBookCommandHandler>> _loggerMock;
   //     private AddBookCommandHandler _handler;
   //
   //     [SetUp]
   //     public void Setup()
   //     {
   //         _bookRepositoryMock = new Mock<IBookRepository>();
   //         _loggerMock = new Mock<ILogger<AddBookCommandHandler>>();
   //         _handler = new AddBookCommandHandler(_bookRepositoryMock.Object, _loggerMock.Object);
   //     }
   //     [Test]
   //     public async Task Handle_ShouldAddBook_WhenBookIsValid()
   //     {
   //         // Arrange
   //         var newBook = new Book(1, "New Book", "This is a new book", new Author(1, "Author Name"));
   //         var command = new AddBookCommand(newBook);
   //
   //         _bookRepositoryMock
   //             .Setup(repo => repo.AddBook(It.IsAny<Book>()))
   //             .ReturnsAsync(OperationResult<Book>.Success(newBook, "Book added successfully."));
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.IsTrue(result.IsSuccess);
   //         Assert.AreEqual("Book added successfully.", result.Message);
   //         _bookRepositoryMock.Verify(repo => repo.AddBook(It.IsAny<Book>()), Times.Once);
   //     }
   //
   //     [Test]
   //     public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
   //     {
   //         // Arrange
   //         var newBook = new Book(1, "New Book", "This is a new book", new Author(1, "Author Name"));
   //         var command = new AddBookCommand(newBook);
   //
   //         _bookRepositoryMock
   //             .Setup(repo => repo.AddBook(It.IsAny<Book>()))
   //             .ReturnsAsync(OperationResult<Book>.Failure("An error occurred while adding the book."));
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.IsFalse(result.IsSuccess);
   //         Assert.AreEqual("An error occurred while adding the book.", result.Message);
   //     }
   // }
}
