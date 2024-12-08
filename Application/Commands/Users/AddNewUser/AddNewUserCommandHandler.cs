using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Users.AddNewUser
{
    public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AddNewUserCommandHandler> _logger;
        

        public AddNewUserCommandHandler(IUserRepository userRepository, ILogger<AddNewUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to add a new user with username: {UserName}", request.NewUser.UserName);

            var userToCreate = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.NewUser.UserName,
            };

            userToCreate.Password = BCrypt.Net.BCrypt.HashPassword(request.NewUser.Password);

            try
            {
                var result = await _userRepository.AddUserAsync(userToCreate);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully added a new user with username: {UserName}", userToCreate.UserName);
                    return OperationResult<User>.Success(userToCreate, "User added successfully.");
                }

                _logger.LogWarning("Failed to add user with username: {UserName}. Error: {ErrorMessage}",
                                   userToCreate.UserName, result.ErrorMessage);

                return OperationResult<User>.Failure(result.ErrorMessage, "Failed to add user.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user with username: {UserName}", userToCreate.UserName);
                return OperationResult<User>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
