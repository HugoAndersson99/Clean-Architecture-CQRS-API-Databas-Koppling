using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAuthorByIdQuery for ID {AuthorId}.", request.Id);

            // Hämta författaren från repository
            var result = await _authorRepository.GetAuthorById(request.Id);

            // Returnera resultatet som OperationResult
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully retrieved author with ID {AuthorId}.", request.Id);
            }
            else
            {
                _logger.LogWarning("Failed to retrieve author with ID {AuthorId}: {ErrorMessage}", request.Id, result.ErrorMessage);
            }

            return result;
        }
    }
}
