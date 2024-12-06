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
            // Logga när vi börjar hantera uppdateringen
            _logger.LogInformation("Handling update request for author with Id {AuthorId}", request.Id);

            // Kalla på repositoryn för att uppdatera författaren
            var result = await _authorRepository.UpdateAuthor(request.UpdatedAuthor, request.Id);

            if (result.IsSuccess)
            {
                // Om uppdateringen lyckas, logga framgång
                _logger.LogInformation("Successfully updated author with Id {AuthorId}", request.Id);
                return result;
            }

            // Om något gick fel, logga misslyckandet
            _logger.LogWarning("Failed to update author with Id {AuthorId}", request.Id);
            return OperationResult<Author>.Failure("Failed to update author", "Update error");
        }
    }
}
