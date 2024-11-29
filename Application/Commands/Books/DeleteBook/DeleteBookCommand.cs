using Domain;
using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookCommand : IRequest<List<Book>>
    {
        public DeleteBookCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
