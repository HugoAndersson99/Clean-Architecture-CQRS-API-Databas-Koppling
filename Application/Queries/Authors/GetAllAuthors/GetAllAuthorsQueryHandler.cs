using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;


namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;
        private readonly IMemoryCache _cache;

        public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository, ILogger<GetAllAuthorsQueryHandler> logger, IMemoryCache cache)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "AllAuthors";

            if (!_cache.TryGetValue(cacheKey, out List<Author> authorsFromDb))
            {
                try
                {
                    var result = await _authorRepository.GetAllAuthors();

                    if (result.Data == null || result.Data.Count == 0)
                    {
                        _logger.LogWarning("No authors found.");
                        return OperationResult<List<Author>>.Failure("No authors found.", "Empty list.");
                    }

                    authorsFromDb = result.Data;

                    _cache.Set(cacheKey, authorsFromDb, TimeSpan.FromMinutes(10));
                    _logger.LogInformation("Authors cached successfully.");

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while fetching authors.");
                    return OperationResult<List<Author>>.Failure("An error occurred while fetching authors.", "Unexpected error.");
                }
            }

            _logger.LogInformation("Returning cached authors.");
            return OperationResult<List<Author>>.Success(authorsFromDb);
        }
    }
}
