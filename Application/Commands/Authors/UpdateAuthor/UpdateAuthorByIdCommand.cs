using Application.ApplicationDtos;
using Domain;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommand : IRequest<OperationResult<Author>>
    {
        public UpdateAuthorByIdCommand(AuthorDto updatedAuthor, int id)
        {
            UpdatedAuthor = updatedAuthor;
            Id = id;
        }

        public AuthorDto UpdatedAuthor { get; }
        public int Id { get; }
    }
}
