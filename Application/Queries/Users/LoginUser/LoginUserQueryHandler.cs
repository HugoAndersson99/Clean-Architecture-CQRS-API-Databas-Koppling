
using Application.Queries.Users.LoginUser.Helpers;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Users.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly FakeDatabase _database;
        private readonly TokenHelper _tokenHelper;

        public LoginUserQueryHandler(FakeDatabase database, TokenHelper tokenHelper)
        {
            _database = database;
            _tokenHelper = tokenHelper;
        }

        public Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = _database.Users.FirstOrDefault(user => user.UserName == request.LoginUser.UserName && user.Password == request.LoginUser.Password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            string token = _tokenHelper.GenerateJwtToken(user);

            return Task.FromResult(token);
        }
    }
}
