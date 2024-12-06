
using Application.ApplicationDtos;
using Domain;

namespace Application.Interfaces.RepositoryInterfaces
{
    public interface IAuthorRepository
    {
        Task<OperationResult<Author>> AddAuthor(AuthorDto author);
        Task<OperationResult<List<Author>>> GetAllAuthors();
        Task<OperationResult<Author>> GetAuthorById(int id);
        Task<OperationResult<string>> DeleteAuthorById(int id);
        Task<OperationResult<Author>> UpdateAuthor(AuthorDto author, int id);
    }
}
