using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, List<Author>>
    {
        private readonly FakeDatabase _database;

        public DeleteAuthorCommandHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<List<Author>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0.", nameof(request.Id));
            }

            Author authorToDelete = _database.Authors.FirstOrDefault(author => author.Id == request.Id);

            if (authorToDelete == null)
            {
                throw new KeyNotFoundException($"No author found with Id {request.Id}.");
            }

            try
            {
                _database.Authors.Remove(authorToDelete);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while trying to delete the author with Id {request.Id}.", ex);
            }

            return Task.FromResult(_database.Authors);
        }
    }
    
}
