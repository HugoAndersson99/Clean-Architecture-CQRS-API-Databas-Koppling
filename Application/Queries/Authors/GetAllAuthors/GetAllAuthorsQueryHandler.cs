using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;

        public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository, ILogger<GetAllAuthorsQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Attempting to get all authors from the database.");
                var authors = await _authorRepository.GetAllAuthors(); // Hämta alla författare från databasen

                if (authors == null || authors.Data.Count == 0)
                {
                    _logger.LogWarning("No authors found in the database.");
                    return OperationResult<List<Author>>.Failure("No authors found.", "Database is empty.");
                }

                _logger.LogInformation($"Successfully retrieved {authors.Data.Count} authors from the database.");
                return OperationResult<List<Author>>.Success(authors.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving authors.");
                return OperationResult<List<Author>>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
