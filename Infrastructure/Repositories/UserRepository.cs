using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RealDatabase _database;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(RealDatabase database, ILogger<UserRepository> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task<OperationResult<User>> AddUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Attempting to add user with username: {UserName}", user.UserName);

                await _database.Users.AddAsync(user);
                await _database.SaveChangesAsync();

                _logger.LogInformation("Successfully added user with username: {UserName}", user.UserName);
                return OperationResult<User>.Success(user, "User added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user with username: {UserName}", user.UserName);
                return OperationResult<User>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
        public async Task<OperationResult<List<User>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _database.Users.ToListAsync();
                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found in the database.");
                    return OperationResult<List<User>>.Failure("No users found.");
                }

                _logger.LogInformation("Successfully retrieved {UserCount} users.", users.Count);
                return OperationResult<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users.");
                return OperationResult<List<User>>.Failure($"An error occurred: {ex.Message}");
            }
        }
        public async Task<OperationResult<User>> LoginUserAsync(string userName, string password)
        {
            try
            {
                var user = await _database.Users.FirstOrDefaultAsync(u => u.UserName == userName);

                if (user == null)
                {
                    _logger.LogWarning("Login failed: No user found with username {UserName}", userName);
                    return OperationResult<User>.Failure("Invalid username or password.");
                }

                if (user.Password != password)
                {
                    _logger.LogWarning("Login failed: Incorrect password for username {UserName}", userName);
                    return OperationResult<User>.Failure("Invalid username or password.");
                }

                _logger.LogInformation("User {UserName} logged in successfully.", userName);
                return OperationResult<User>.Success(user, "Login successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in user {UserName}.", userName);
                return OperationResult<User>.Failure($"An error occurred: {ex.Message}");
            }
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _database.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
