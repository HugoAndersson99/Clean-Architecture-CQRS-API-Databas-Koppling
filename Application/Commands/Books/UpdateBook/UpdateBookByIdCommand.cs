using Application.ApplicationDtos;
using Domain;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommand : IRequest<OperationResult<Book>>
    {
        public int BookId { get; set; }
        public BookDto UpdatedBook { get; set; }

        public UpdateBookByIdCommand(int bookId, BookDto updatedBook)
        {
            BookId = bookId;
            UpdatedBook = updatedBook;
        }
    }
}
