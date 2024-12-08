using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;

        public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository, ILogger<GetAllAuthorsQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authorRepository.GetAllAuthors();

                if (result.Data == null || result.Data.Count == 0)
                {
                    _logger.LogWarning("No authors found.");
                    return OperationResult<List<Author>>.Failure("No authors found.", "Empty list.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching authors.");
                return OperationResult<List<Author>>.Failure("An error occurred while fetching authors.", "Unexpected error.");
            }
        }
    }
}
