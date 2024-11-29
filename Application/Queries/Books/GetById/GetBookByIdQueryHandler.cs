using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Books.GetById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly FakeDatabase _database;

        public GetBookByIdQueryHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0.", nameof(request.Id));
            }

            Book requestedBook = _database.Books.FirstOrDefault(book => book.Id == request.Id);

            if (requestedBook == null)
            {
                throw new KeyNotFoundException($"Book with Id {request.Id} not found.");
            }

            return Task.FromResult(requestedBook);
        }
    }
}
