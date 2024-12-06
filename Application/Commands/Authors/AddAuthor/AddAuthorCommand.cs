using Application.ApplicationDtos;
using Domain;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommand : IRequest<OperationResult<Author>>
    {
        public AddAuthorCommand(AuthorDto authorToAdd)
        {
            AuthorToAdd = authorToAdd;
        }

        public AuthorDto AuthorToAdd { get; }
    }
}
