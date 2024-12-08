using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Queries.Books.GetAll
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<List<Book>>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<GetAllBooksQueryHandler> _logger;

        public GetAllBooksQueryHandler(IBookRepository bookRepository, ILogger<GetAllBooksQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }


        public async Task<OperationResult<List<Book>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to get all books.");

            try
            {
                var result = await _bookRepository.GetAllBooks();

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved all books.");
                    return result;
                }

                _logger.LogWarning("Failed to retrieve books.");
                return OperationResult<List<Book>>.Failure("Failed to retrieve books.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all books.");
                return OperationResult<List<Book>>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
