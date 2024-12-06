using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OperationResult<List<User>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IUserRepository userRepository, ILogger<GetAllUsersQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUsersQuery.");

            // Hämta användare från repository
            var result = await _userRepository.GetAllUsersAsync();

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully retrieved {UserCount} users.", result.Data.Count);
            }
            else
            {
                _logger.LogWarning("Failed to retrieve users: {ErrorMessage}", result.ErrorMessage);
            }

            return result;
        }
    }
}
