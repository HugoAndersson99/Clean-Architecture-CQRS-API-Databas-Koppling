﻿
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.LoginUser.Helpers;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Users.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginUserQueryHandler> _logger;
        private readonly TokenHelper _tokenHelper;

        public LoginUserQueryHandler(IUserRepository userRepository, ILogger<LoginUserQueryHandler> logger, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _tokenHelper = tokenHelper;
        }
        public async Task<OperationResult<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Hämta användaren från databasen baserat på användarnamn
                var user = await _userRepository.GetUserByUsernameAsync(request.LoginUser.UserName);

                if (user == null || user.Password != request.LoginUser.Password)  // Direkt jämförelse av lösenord
                {
                    _logger.LogWarning("Invalid login attempt for username: {UserName}", request.LoginUser.UserName);
                    return OperationResult<string>.Failure("Invalid username or password");
                }

                // Generera JWT-token
                string token = _tokenHelper.GenerateJwtToken(user);

                _logger.LogInformation("User {UserName} successfully logged in.", request.LoginUser.UserName);

                return OperationResult<string>.Success(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to log in user: {UserName}", request.LoginUser.UserName);
                return OperationResult<string>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
