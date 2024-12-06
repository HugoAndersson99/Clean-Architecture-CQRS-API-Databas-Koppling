

using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Users.AddNewUser
{
    public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;

        public AddNewUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<User>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            var userToCreate = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.NewUser.UserName,
                Password = request.NewUser.Password
            };

            try
            {
                var result = await _userRepository.AddUserAsync(userToCreate);

                if (result.IsSuccess)
                {
                    return OperationResult<User>.Success(userToCreate, "User added successfully.");
                }

                return OperationResult<User>.Failure(result.ErrorMessage, "Failed to add user.");
            }
            catch (Exception ex)
            {
                return OperationResult<User>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
