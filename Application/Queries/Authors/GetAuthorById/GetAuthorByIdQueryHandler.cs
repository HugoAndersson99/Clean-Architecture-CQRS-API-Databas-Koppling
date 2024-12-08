using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;
        private readonly IMemoryCache _cache;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, ILogger<GetAuthorByIdQueryHandler> logger, IMemoryCache cache)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<OperationResult<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"Author_{request.Id}";

            if (_cache.TryGetValue(cacheKey, out Author cachedAuthor))
            {
                _logger.LogInformation("Returning cached author with Id: {AuthorId}", request.Id);
                return OperationResult<Author>.Success(cachedAuthor);
            }

            _logger.LogInformation("Attempting to get author by Id: {AuthorId}", request.Id);

            try
            {
                var result = await _authorRepository.GetAuthorById(request.Id);

                if (result.IsSuccess && result.Data != null)
                {
                    _logger.LogInformation("Successfully retrieved author with Id: {AuthorId}", request.Id);

                    _cache.Set(cacheKey, result.Data, TimeSpan.FromMinutes(5));
                    _logger.LogInformation("Author cached with Id: {AuthorId}", request.Id);

                    return result;
                }

                _logger.LogWarning("Failed to retrieve author with Id: {AuthorId}", request.Id);
                return OperationResult<Author>.Failure("Author not found.", "Database error.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting author by Id: {AuthorId}", request.Id);
                return OperationResult<Author>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
