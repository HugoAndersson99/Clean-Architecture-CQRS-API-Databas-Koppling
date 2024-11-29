using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<List<Author>>
    {

    }
}
