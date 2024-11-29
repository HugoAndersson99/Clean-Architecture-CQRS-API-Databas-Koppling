using Domain;
using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommand : IRequest<List<Book>>
    {
        public AddBookCommand(Book bookToAdd)
        {
            newBook = bookToAdd;
        }


        public Book newBook { get; }
    }
}
