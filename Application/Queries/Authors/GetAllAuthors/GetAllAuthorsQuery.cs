using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<OperationResult<List<Author>>>
    {

    }
}
