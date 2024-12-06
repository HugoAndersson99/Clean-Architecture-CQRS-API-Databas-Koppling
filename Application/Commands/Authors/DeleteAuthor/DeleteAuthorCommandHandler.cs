using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult<string>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<DeleteAuthorCommandHandler> _logger;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<DeleteAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting DeleteAuthorCommand for AuthorId: {AuthorId}", request.Id);

            try
            {
                var result = await _authorRepository.DeleteAuthorById(request.Id);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully deleted author with Id: {AuthorId}", request.Id);
                    return OperationResult<string>.Success("Author deleted successfully.", "Delete operation successful.");
                }

                _logger.LogWarning("Failed to delete author with Id: {AuthorId}. Reason: {ErrorMessage}", request.Id, result.ErrorMessage);
                return OperationResult<string>.Failure(result.ErrorMessage, "Delete operation failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting author with Id: {AuthorId}", request.Id);
                return OperationResult<string>.Failure("An unexpected error occurred.", "Delete operation encountered an error.");
            }
        }
    }
}
