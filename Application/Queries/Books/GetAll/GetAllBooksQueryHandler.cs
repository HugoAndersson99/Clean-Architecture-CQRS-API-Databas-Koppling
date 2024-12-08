using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;


namespace Application.Queries.Books.GetAll
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<List<Book>>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<GetAllBooksQueryHandler> _logger;
        private readonly IMemoryCache _cache;

        public GetAllBooksQueryHandler(IBookRepository bookRepository, ILogger<GetAllBooksQueryHandler> logger, IMemoryCache cache)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<OperationResult<List<Book>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "AllBooks";

            if (!_cache.TryGetValue(cacheKey, out List<Book> booksFromDb))
            {
                try
                {
                    var result = await _bookRepository.GetAllBooks();

                    if (!result.IsSuccess || result.Data == null || result.Data.Count == 0)
                    {
                        _logger.LogWarning("No books found.");
                        return OperationResult<List<Book>>.Failure("No books found.", "Empty list.");
                    }

                    booksFromDb = result.Data;

                    _cache.Set(cacheKey, booksFromDb, TimeSpan.FromMinutes(2));
                    _logger.LogInformation("Books cached successfully.");

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while fetching books.");
                    return OperationResult<List<Book>>.Failure("An error occurred while fetching books.", "Unexpected error.");
                }
            }

            _logger.LogInformation("Returning cached books.");
            return OperationResult<List<Book>>.Success(booksFromDb);
        }
    }
}
