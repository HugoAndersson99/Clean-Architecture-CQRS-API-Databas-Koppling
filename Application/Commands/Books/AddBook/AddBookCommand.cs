using Application.ApplicationDtos;
using Domain;
using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommand : IRequest<OperationResult<Book>>
    {
        public AddBookCommand(BookDto bookToAdd)
        {
            NewBook = bookToAdd;
        }

        public BookDto NewBook { get; }
    }
}
