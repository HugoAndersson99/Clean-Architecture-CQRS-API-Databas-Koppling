
using Application.ApplicationDtos;
using Domain;

namespace Application.Interfaces.RepositoryInterfaces
{
    public interface IBookRepository
    {
        Task<OperationResult<Book>> AddBook(BookDto book);
        Task<OperationResult<Book>> DeleteBook(int bookId);
        Task<OperationResult<Book>> UpdateBook(int bookId, BookDto updatedBook);
        Task<OperationResult<List<Book>>> GetAllBooks();
        Task<OperationResult<Book>> GetBookById(int id);
    }
}
