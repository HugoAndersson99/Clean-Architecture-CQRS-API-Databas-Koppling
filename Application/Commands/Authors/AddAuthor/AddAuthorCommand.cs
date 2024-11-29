using Domain;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommand : IRequest<List<Author>>
    {
        public AddAuthorCommand(Author authorToAdd)
        {
            AuthorToAdd = authorToAdd;
        }

        public Author AuthorToAdd { get; }
    }
}
