

using Application.ApplicationDtos;
using Domain;
using MediatR;

namespace Application.Queries.Users.LoginUser
{
    public class LoginUserQuery : IRequest<OperationResult<string>>
    {
        public LoginUserQuery(UserDto loginUser)
        {
            LoginUser = loginUser;
        }

        public UserDto LoginUser { get; set; }
    }
}
