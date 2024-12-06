using Domain;
using MediatR;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<OperationResult<string>>
    {
        public DeleteAuthorCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
