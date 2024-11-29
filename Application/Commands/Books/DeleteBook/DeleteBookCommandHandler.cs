using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, List<Book>>
    {
        private readonly FakeDatabase _database;

        public DeleteBookCommandHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<List<Book>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0.", nameof(request.Id));
            }

            Book bookToDelete = _database.Books.FirstOrDefault(book => book.Id == request.Id);

            if (bookToDelete == null)
            {
                throw new KeyNotFoundException($"No book found with Id {request.Id}.");
            }

            try
            {
                if (bookToDelete.Author != null)
                {
                    bookToDelete.Author.Books.Remove(bookToDelete);
                }

                _database.Books.Remove(bookToDelete);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while trying to delete the book with Id {request.Id}.", ex);
            }

            return Task.FromResult(_database.Books);
        }
    }
}
