

using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly FakeDatabase _database;

        public GetAllUsersQueryHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            List<User> AllUsersFromDb = _database.Users;
            return Task.FromResult(AllUsersFromDb);
        }
    }
}
