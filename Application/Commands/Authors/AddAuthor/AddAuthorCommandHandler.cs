using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, OperationResult<Author>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AddAuthorCommandHandler> _logger;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<AddAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing AddAuthorCommand for Author: {AuthorName}", request.AuthorToAdd?.Name);
            if (string.IsNullOrEmpty(request.AuthorToAdd?.Name))
            {
                // Om författarens namn är tomt eller null, returnera ett fel
                return OperationResult<Author>.Failure("Author name cannot be empty");
            }

            try
            {
                var result = await _authorRepository.AddAuthor(request.AuthorToAdd);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully added author: {AuthorName})", request.AuthorToAdd.Name);
                    return OperationResult<Author>.Success(result.Data, "Author added successfully.");
                }

                _logger.LogWarning("Failed to add author: {AuthorName}", request.AuthorToAdd.Name);
                return OperationResult<Author>.Failure("Failed to add author.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding author: {AuthorName}", request.AuthorToAdd.Name);
                return OperationResult<Author>.Failure($"An error occurred: {ex.Message}", "Database error.");
            }
        }
    }
}
