

using Application.ApplicationDtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly RealDatabase _database;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(RealDatabase database, ILogger<BookRepository> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> AddBook(BookDto bookDto)
        {
            try
            {
                Author existingAuthor = null;

                if (bookDto.Author != null)
                {
                    // Försök hitta en författare med samma namn
                    existingAuthor = _database.Authors.FirstOrDefault(a => a.Name == bookDto.Author.Name);

                    if (existingAuthor == null)
                    {
                        // Lägg till ny författare om den inte finns
                        existingAuthor = new Author
                        {
                            Name = bookDto.Author.Name
                        };

                        _database.Authors.Add(existingAuthor);
                    }
                }

                // Skapa ny bok och koppla till författaren
                var newBook = new Book
                {
                    Title = bookDto.Title,
                    Description = bookDto.Description,
                    Author = existingAuthor
                };

                _database.Books.Add(newBook);

                // Spara ändringar
                await _database.SaveChangesAsync();

                return OperationResult<Book>.Success(newBook, "Book added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding book to the database.");
                return OperationResult<Book>.Failure("An error occurred while adding the book.");
            }
        }
        public async Task<OperationResult<Book>> DeleteBook(int bookId)
        {
            try
            {
                var bookToDelete = await _database.Books.FindAsync(bookId);

                if (bookToDelete == null)
                {
                    return OperationResult<Book>.Failure("Book not found.", "Database error.");
                }

                _database.Books.Remove(bookToDelete);
                await _database.SaveChangesAsync();

                return OperationResult<Book>.Success(null, "Book deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting book from the database.");
                return OperationResult<Book>.Failure("An error occurred while deleting the book.");
            }
        }
        public async Task<OperationResult<Book>> UpdateBook(int bookId, BookDto updatedBook)
        {
            try
            {
                var existingBook = await _database.Books.FindAsync(bookId);

                if (existingBook == null)
                {
                    return OperationResult<Book>.Failure("Book not found.", "Database error.");
                }

                // Uppdatera bokens egenskaper
                existingBook.Title = updatedBook.Title;
                existingBook.Description = updatedBook.Description;

                // Om författaren uppdateras också
                if (updatedBook.Author != null)
                {
                    // Försök hitta en befintlig författare baserat på namn
                    var existingAuthor = await _database.Authors
                        .FirstOrDefaultAsync(a => a.Name == updatedBook.Author.Name);

                    if (existingAuthor == null)
                    {
                        // Om författaren inte finns, skapa en ny
                        existingAuthor = new Author
                        {
                            Name = updatedBook.Author.Name
                        };
                        _database.Authors.Add(existingAuthor);
                    }

                    // Koppla den hittade eller nya författaren till boken
                    existingBook.Author = existingAuthor;
                }

                await _database.SaveChangesAsync();

                return OperationResult<Book>.Success(existingBook, "Book updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating book in the database.");
                return OperationResult<Book>.Failure("An error occurred while updating the book.");
            }
        }
        public async Task<OperationResult<List<Book>>> GetAllBooks()
        {
            try
            {
                var books = await _database.Books
                    .Include(b => b.Author) // Hämta med författare
                    .ToListAsync();

                if (books == null || books.Count == 0)
                {
                    return OperationResult<List<Book>>.Failure("No books found.", "Database error.");
                }

                return OperationResult<List<Book>>.Success(books, "Books retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving books from the database.");
                return OperationResult<List<Book>>.Failure("An error occurred while retrieving books.");
            }
        }
        public async Task<OperationResult<Book>> GetBookById(int id)
        {
            try
            {
                var book = await _database.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                {
                    return OperationResult<Book>.Failure("No book found with the given Id.", "Database error.");
                }

                return OperationResult<Book>.Success(book, "Book retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving book with Id: {BookId}", id);
                return OperationResult<Book>.Failure("An error occurred while retrieving the book.");
            }
        }
    }
}
