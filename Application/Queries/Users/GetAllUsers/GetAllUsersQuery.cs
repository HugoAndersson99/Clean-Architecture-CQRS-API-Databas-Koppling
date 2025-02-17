﻿using Domain;
using MediatR;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<OperationResult<List<User>>>
    {
    }
}
