using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult<Book>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<DeleteBookCommandHandler> _logger;

        public DeleteBookCommandHandler(IBookRepository bookRepository, ILogger<DeleteBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete book with ID: {BookId}", request.BookId);

            try
            {
                var result = await _bookRepository.DeleteBook(request.BookId);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully deleted book with ID: {BookId}", request.BookId);
                    return OperationResult<Book>.Success(null, "Book deleted successfully.");
                }

                _logger.LogWarning("Failed to delete book with ID: {BookId}", request.BookId);
                return OperationResult<Book>.Failure("Failed to delete book.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting book with ID: {BookId}", request.BookId);
                return OperationResult<Book>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
