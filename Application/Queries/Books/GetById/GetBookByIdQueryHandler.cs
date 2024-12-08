using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Books.GetById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, OperationResult<Book>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<GetBookByIdQueryHandler> _logger;

        public GetBookByIdQueryHandler(IBookRepository bookRepository, ILogger<GetBookByIdQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to get book by Id: {BookId}", request.Id);

            try
            {
                var result = await _bookRepository.GetBookById(request.Id);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved book with Id: {BookId}", request.Id);
                    return result;
                }

                _logger.LogWarning("Failed to retrieve book with Id: {BookId}", request.Id);
                return OperationResult<Book>.Failure("Book not found.", "Database error.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting book by Id: {BookId}", request.Id);
                return OperationResult<Book>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
