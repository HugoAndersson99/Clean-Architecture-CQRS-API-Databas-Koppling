using Domain;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommand : IRequest<Author>
    {
        public UpdateAuthorByIdCommand(Author updatedAuthor, int id)
        {
            UpdatedAuthor = updatedAuthor;
            Id = id;
        }

        public Author UpdatedAuthor { get; }
        public int Id { get; }
    }
}
