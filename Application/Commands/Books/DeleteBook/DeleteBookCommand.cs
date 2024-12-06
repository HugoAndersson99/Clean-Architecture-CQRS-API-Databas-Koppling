using Domain;
using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookCommand : IRequest<OperationResult<Book>>
    {
        public int BookId { get; set; }

        public DeleteBookCommand(int bookId)
        {
            BookId = bookId;
        }
    }
}
