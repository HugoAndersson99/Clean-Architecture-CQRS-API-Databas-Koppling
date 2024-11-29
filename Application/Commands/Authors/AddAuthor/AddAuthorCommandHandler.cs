using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, List<Author>>
    {
        private readonly FakeDatabase _database;

        public AddAuthorCommandHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<List<Author>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            Author newAuthor = request.AuthorToAdd;
            if (newAuthor == null)
            {
                throw new ArgumentNullException(nameof(newAuthor), "Author details must be provided.");
            }

            if (string.IsNullOrWhiteSpace(newAuthor.Name))
            {
                throw new ArgumentException("Author name must not be empty.", nameof(newAuthor.Name));
            }

            // Generera ett nytt ID om det behövs
            if (_database.Authors.Count != 0)
            {
                newAuthor.Id = _database.Authors.Max(a => a.Id) + 1;
            }
            else
            {
                newAuthor.Id = 1;
            }

            _database.Authors.Add(newAuthor);

            return Task.FromResult(_database.Authors);
        }
    }
}
