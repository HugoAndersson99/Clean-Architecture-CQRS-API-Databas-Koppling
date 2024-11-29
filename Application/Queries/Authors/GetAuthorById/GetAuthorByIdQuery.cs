using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAuthorById
{
    public class GetAuthorByIdQuery : IRequest<Author>
    {
        public int Id { get; }
        public GetAuthorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
