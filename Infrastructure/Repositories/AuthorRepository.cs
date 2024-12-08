
using Application.ApplicationDtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly RealDatabase _database;
        private readonly ILogger<AuthorRepository> _logger;

        public AuthorRepository(RealDatabase database, ILogger<AuthorRepository> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> AddAuthor(AuthorDto authorDto)
        {
            try
            {
                if (authorDto == null || string.IsNullOrWhiteSpace(authorDto.Name))
                {
                    return OperationResult<Author>.Failure("Author data is invalid");
                }

                var newAuthor = new Author
                {
                    Name = authorDto.Name
                };
                _database.Authors.Add(newAuthor);
                await _database.SaveChangesAsync();

                return OperationResult<Author>.Success(newAuthor, "Author added");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding author to the database");
                return OperationResult<Author>.Failure("An error occurred while adding the author");
            }
        }

        public async Task<OperationResult<string>> DeleteAuthorById(int id)
        {
            _logger.LogInformation("Attempting to delete author with Id: {AuthorId}", id);

            try
            {
                var authorToDelete = await _database.Authors.FindAsync(id);

                if (authorToDelete == null)
                {
                    _logger.LogWarning("Author with Id: {AuthorId} not found.", id);
                    return OperationResult<string>.Failure($"Author with Id {id} not found.", "Entity not found.");
                }

                _database.Authors.Remove(authorToDelete);
                await _database.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted author with Id: {AuthorId}", id);
                return OperationResult<string>.Success("Author deleted successfully.", "Delete operation successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting author with Id: {AuthorId}", id);
                return OperationResult<string>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }

        public async Task<OperationResult<List<Author>>> GetAllAuthors()
        {
            try
            {
                var authorsFromDb = await _database.Authors
                    .Include(a => a.Books)
                    .ToListAsync();

                var authorsWithBooks = authorsFromDb.Select(a => new Author
                {
                    Id = a.Id,
                    Name = a.Name,
                    Books = a.Books.Select(b => new Book
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description
                    }).ToList()
                }).ToList();

                if (authorsWithBooks == null || authorsWithBooks.Count == 0)
                {
                    _logger.LogWarning("No authors found in the database.");
                    return OperationResult<List<Author>>.Failure("No authors found.", "Database is empty.");
                }

                _logger.LogInformation($"Successfully retrieved {authorsWithBooks.Count} authors.");
                return OperationResult<List<Author>>.Success(authorsWithBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving authors from the database.");
                return OperationResult<List<Author>>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }

        public async Task<OperationResult<Author>> GetAuthorById(int id)
        {
            try
            {
                var authorFromDb = await _database.Authors
                    .Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (authorFromDb == null)
                {
                    _logger.LogWarning($"No author found with Id {id}.");
                    return OperationResult<Author>.Failure("Author not found.", "Invalid Id.");
                }

                var authorDto = new Author
                {
                    Id = authorFromDb.Id,
                    Name = authorFromDb.Name,
                    Books = authorFromDb.Books.Select(b => new Book
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description
                    }).ToList()
                };

                _logger.LogInformation($"Successfully retrieved author with Id {id}.");
                return OperationResult<Author>.Success(authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving author by Id.");
                return OperationResult<Author>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }

        public async Task<OperationResult<Author>> UpdateAuthor(AuthorDto updatedAuthor, int id)
        {
            try
            {
                var existingAuthor = await _database.Authors.FindAsync(id);

                if (existingAuthor == null)
                {
                    _logger.LogWarning("Author with Id {AuthorId} not found for update", id);
                    return OperationResult<Author>.Failure("Author not found", "Update failed");
                }

                _logger.LogInformation("Attempting to update author with Id {AuthorId}", id);

                existingAuthor.Name = updatedAuthor.Name;

                await _database.SaveChangesAsync();

                _logger.LogInformation("Successfully updated author with Id {AuthorId}", id);

                return OperationResult<Author>.Success(existingAuthor, "Author updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating author with Id {AuthorId}", id);
                return OperationResult<Author>.Failure($"An error occurred: {ex.Message}", "Update error");
            }
        }
    }
}
