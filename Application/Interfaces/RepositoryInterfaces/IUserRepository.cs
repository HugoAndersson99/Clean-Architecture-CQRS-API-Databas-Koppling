

using Domain;

namespace Application.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<OperationResult<User>> AddUserAsync(User user);
        Task<OperationResult<List<User>>> GetAllUsersAsync();
        Task<OperationResult<User>> LoginUserAsync(string userName, string password);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
