using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<Author>>
    {
        private readonly FakeDatabase _database;

        public GetAllAuthorsQueryHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<List<Author>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            if (_database.Authors == null || _database.Authors.Count == 0)
            {
                throw new InvalidOperationException("No authors found in the database.");
            }

            List<Author> authorsFromDB = _database.Authors;

            return Task.FromResult(authorsFromDB);
        }
    }
}
