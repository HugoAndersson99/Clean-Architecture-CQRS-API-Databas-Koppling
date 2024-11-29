using Domain;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommand : IRequest<List<Book>>
    {
        public UpdateBookByIdCommand(Book updatedBook, int id)
        {
            UpdatedBook = updatedBook;
            Id = id;
        }

        public Book UpdatedBook { get; }
        public int Id { get; }
    }
}
