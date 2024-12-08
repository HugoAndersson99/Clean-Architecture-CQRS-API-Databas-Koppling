using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommandHandler : IRequestHandler<UpdateAuthorByIdCommand, OperationResult<Author>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<UpdateAuthorByIdCommandHandler> _logger;

        public UpdateAuthorByIdCommandHandler(IAuthorRepository authorRepository, ILogger<UpdateAuthorByIdCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(UpdateAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling update request for author with Id {AuthorId}", request.Id);

            try
            {
                var result = await _authorRepository.UpdateAuthor(request.UpdatedAuthor, request.Id);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully updated author with Id {AuthorId}", request.Id);
                    return result;
                }

                _logger.LogWarning("Failed to update author with Id {AuthorId}. Reason: {ErrorMessage}", request.Id, result.ErrorMessage);
                return OperationResult<Author>.Failure($"Failed to update author: {result.ErrorMessage}", "Update error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating author with Id {AuthorId}", request.Id);
                return OperationResult<Author>.Failure("An unexpected error occurred while updating the author.", "Unexpected error");
            }
        }
    }
}
