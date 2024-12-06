
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
                // Hämta alla författare med deras böcker
                var authors = await _database.Authors
                    .Include(a => a.Books) // Inkludera böcker kopplade till författaren
                    .ToListAsync();

                // Skapa ett resultat utan att inkludera författarinformation i böckerna
                var authorsWithBooks = authors.Select(a => new Author
                {
                    Id = a.Id,
                    Name = a.Name,
                    Books = a.Books.Select(b => new Book
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description
                        // Här tar vi bort Author-objektet från boken för att slippa redundans
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
                // Hämta författaren med dess böcker
                var author = await _database.Authors
                    .Include(a => a.Books) // Inkludera böcker kopplade till författaren
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (author == null)
                {
                    _logger.LogWarning($"No author found with Id {id}.");
                    return OperationResult<Author>.Failure("Author not found.", "Invalid Id.");
                }

                // Skapa AuthorDto med böcker, men utan författare i böckerna
                var authorDto = new Author
                {
                    Id = author.Id,
                    Name = author.Name,
                    Books = author.Books.Select(b => new Book
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description
                        // Här behåller vi inte författaren i boken
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

                // Logga när vi börjar uppdatera
                _logger.LogInformation("Attempting to update author with Id {AuthorId}", id);

                existingAuthor.Name = updatedAuthor.Name;

                // Om vi har böcker att uppdatera
               //if (updatedAuthor.Books != null)
               //{
               //    existingAuthor.Books = updatedAuthor.Books;
               //}

                await _database.SaveChangesAsync();

                // Logga framgång
                _logger.LogInformation("Successfully updated author with Id {AuthorId}", id);

                return OperationResult<Author>.Success(existingAuthor, "Author updated successfully.");
            }
            catch (Exception ex)
            {
                // Logga fel om något går fel
                _logger.LogError(ex, "Error while updating author with Id {AuthorId}", id);
                return OperationResult<Author>.Failure($"An error occurred: {ex.Message}", "Update error");
            }
        }
    }
}
