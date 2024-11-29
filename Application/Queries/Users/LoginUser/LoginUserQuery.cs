

using Application.ApplicationDtos;
using MediatR;

namespace Application.Queries.Users.LoginUser
{
    public class LoginUserQuery : IRequest<string>
    {
        public LoginUserQuery(UserDto loginUser)
        {
            LoginUser = loginUser;
        }

        public UserDto LoginUser { get; set; }
    }
}
