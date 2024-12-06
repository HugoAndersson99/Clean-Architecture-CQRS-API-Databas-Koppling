using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommandHandler : IRequestHandler<UpdateBookByIdCommand, OperationResult<Book>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<UpdateBookByIdCommandHandler> _logger;

        public UpdateBookByIdCommandHandler(IBookRepository bookRepository, ILogger<UpdateBookByIdCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(UpdateBookByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update book with ID: {BookId}", request.BookId);

            try
            {
                // Anropa repository för att uppdatera boken
                var result = await _bookRepository.UpdateBook(request.BookId, request.UpdatedBook);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully updated book with ID: {BookId}", request.BookId);
                    return result;
                }

                _logger.LogWarning("Failed to update book with ID: {BookId}", request.BookId);
                return OperationResult<Book>.Failure("Failed to update book.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating book with ID: {BookId}", request.BookId);
                return OperationResult<Book>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
