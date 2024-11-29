using Domain;
using MediatR;
using Infrastructure.Database;


namespace Application.Commands.Books.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, List<Book>>
    {
        private readonly FakeDatabase _database;

        public AddBookCommandHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<List<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.newBook == null)
                throw new ArgumentNullException(nameof(request.newBook));

            Book newBook = request.newBook;

            if (_database.Books.Any(book => book.Id == newBook.Id))
                throw new InvalidOperationException($"A book with Id {newBook.Id} already exists.");

            if (newBook.Author != null)
            {
                var existingAuthor = _database.Authors.FirstOrDefault(a => a.Id == newBook.Author.Id);

                if (existingAuthor == null)
                {
                    existingAuthor = newBook.Author;
                    existingAuthor.Id = _database.Authors.Any() ? _database.Authors.Max(a => a.Id) + 1 : 1;
                    _database.Authors.Add(existingAuthor);
                }

                if (!existingAuthor.Books.Contains(newBook))
                {
                    existingAuthor.Books.Add(newBook);
                }

                newBook.Author = existingAuthor;
            }

            newBook.Id = _database.Books.Any() ? _database.Books.Max(b => b.Id) + 1 : 1;

            _database.Books.Add(newBook);

            return Task.FromResult(_database.Books);
        }
    }
}
