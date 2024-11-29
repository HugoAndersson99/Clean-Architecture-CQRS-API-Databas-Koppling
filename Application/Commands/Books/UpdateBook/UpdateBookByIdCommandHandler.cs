using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommandHandler : IRequestHandler<UpdateBookByIdCommand, List<Book>>
    {
        private readonly FakeDatabase _database;

        public UpdateBookByIdCommandHandler(FakeDatabase database)
        {
            _database = database;
        }

        public Task<List<Book>> Handle(UpdateBookByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0.", nameof(request.Id));
            }

            if (request.UpdatedBook == null)
            {
                throw new ArgumentNullException(nameof(request.UpdatedBook), "Updated book details must be provided.");
            }

            Book bookToUpdate = _database.Books.FirstOrDefault(book => book.Id == request.Id);

            if (bookToUpdate == null)
            {
                throw new KeyNotFoundException($"Book with Id {request.Id} not found.");
            }

            try
            {
                bookToUpdate.Description = request.UpdatedBook.Description;
                bookToUpdate.Title = request.UpdatedBook.Title;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while updating the book with Id {request.Id}.", ex);
            }

            return Task.FromResult(_database.Books);
        }
    }
}
