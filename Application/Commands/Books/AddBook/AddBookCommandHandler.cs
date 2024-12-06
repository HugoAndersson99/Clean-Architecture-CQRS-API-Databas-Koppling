using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Commands.Books.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, OperationResult<Book>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<AddBookCommandHandler> _logger;

        public AddBookCommandHandler(IBookRepository bookRepository, ILogger<AddBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to add a new book: {BookTitle}", request.NewBook.Title);

            try
            {
                
                // Anropa repository för att lägga till boken
                var result = await _bookRepository.AddBook(request.NewBook);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully added book: {BookTitle} (ID: {BookId})", request.NewBook.Title, result.Data.Id);
                    return OperationResult<Book>.Success(result.Data, "Book added successfully.");
                }

                _logger.LogWarning("Failed to add book: {BookTitle}", request.NewBook.Title);
                return OperationResult<Book>.Failure("Failed to add book.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding book: {BookTitle}", request.NewBook.Title);
                return OperationResult<Book>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
