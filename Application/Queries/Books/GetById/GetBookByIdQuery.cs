using Domain;
using MediatR;

namespace Application.Queries.Books.GetById
{
    public class GetBookByIdQuery : IRequest<Book>
    {
        public GetBookByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

    }
}
