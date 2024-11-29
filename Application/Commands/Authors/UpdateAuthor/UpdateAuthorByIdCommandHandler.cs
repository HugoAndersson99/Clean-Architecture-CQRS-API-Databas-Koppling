using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommandHandler : IRequestHandler<UpdateAuthorByIdCommand, Author>
    {
        private readonly FakeDatabase _database;

        public UpdateAuthorByIdCommandHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<Author> Handle(UpdateAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.UpdatedAuthor == null)
            {
                throw new ArgumentNullException(nameof(request.UpdatedAuthor), "Updated author details must be provided.");
            }

            if (string.IsNullOrWhiteSpace(request.UpdatedAuthor.Name))
            {
                throw new ArgumentException("Author name must not be empty.", nameof(request.UpdatedAuthor.Name));
            }

            Author authorToUpdate = _database.Authors.FirstOrDefault(author => author.Id == request.Id);

            if (authorToUpdate == null)
            {
                throw new KeyNotFoundException($"Author with Id {request.Id} not found.");
            }

            authorToUpdate.Name = request.UpdatedAuthor.Name;

            if (request.UpdatedAuthor.Books != null)
            {
                authorToUpdate.Books = request.UpdatedAuthor.Books;
            }

            return Task.FromResult(authorToUpdate);
        }

        
    }
}
