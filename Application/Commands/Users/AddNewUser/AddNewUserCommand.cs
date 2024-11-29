﻿



using Application.ApplicationDtos;
using Domain;
using MediatR;

namespace Application.Commands.Users.AddNewUser
{
    public class AddNewUserCommand : IRequest<User>
    {
        public AddNewUserCommand(UserDto newUser)
        {
            NewUser = newUser;
        }

        public UserDto NewUser { get; set; }
    }
}
