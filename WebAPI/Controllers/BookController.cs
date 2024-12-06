using Domain;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Commands.Books.AddBook;
using Application.Queries.Books.GetAll;
using Application.Commands.Books.DeleteBook;
using Application.Commands.Books.UpdateBook;
using Application.Queries.Books.GetById;
using Microsoft.AspNetCore.Authorization;
using Application.ApplicationDtos;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookController> _logger;

        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        // GET: api/<BookController>
        [Authorize]
        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IActionResult> GetAllBooksFromDB()
        {
            var operationResult = await _mediator.Send(new GetAllBooksQuery());

            if (operationResult.IsSuccess)
            {
                var booksWithAuthors = operationResult.Data.Select(book => new
                {
                    book.Id,
                    book.Title,
                    book.Description,
                    Author = book.Author != null ? new { book.Author.Id, book.Author.Name } : null
                });

                return Ok(booksWithAuthors);
            }

            _logger.LogWarning("Failed to retrieve all books. Error: {ErrorMessage}", operationResult.Message);
            return BadRequest(operationResult.Message);
        }

        // GET api/<BookController>/5
        [Authorize]
        [HttpGet]
        [Route("getBookById/{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var result = await _mediator.Send(new GetBookByIdQuery(bookId));

            if (result.IsSuccess)
            {
                var bookWithAuthor = new
                {
                    result.Data.Id,
                    result.Data.Title,
                    result.Data.Description,
                    Author = result.Data.Author != null ? new { result.Data.Author.Id, result.Data.Author.Name } : null
                };

                return Ok(bookWithAuthor);
            }

            _logger.LogWarning("Failed to retrieve book with Id: {BookId}. Error: {ErrorMessage}", bookId, result.Message);
            return NotFound(result.Message);
        }

        // POST api/<BookController>
        [Authorize]
        [HttpPost]
        [Route("AddNewBook")]
        public async Task<IActionResult> Post([FromBody] BookDto bookToAdd)
        {
            if (bookToAdd == null)
            {
                _logger.LogWarning("Book details were not provided.");
                return BadRequest("Book details must be provided.");
            }

            var result = await _mediator.Send(new AddBookCommand(bookToAdd));

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully added new book with Id: {BookId}", result.Data.Id);
                return CreatedAtAction(nameof(GetBookById), new { bookId = result.Data.Id }, result.Data);
            }

            _logger.LogWarning("Failed to add book: {ErrorMessage}", result.Message);
            return BadRequest(result.Message);
        }

        // PUT api/<BookController>/5
        [Authorize]
        [HttpPut]
        [Route("updateBook/{updatedBookId}")]
        public async Task<IActionResult> UpdateBook([FromBody] BookDto updatedBook, int updatedBookId)
        {
            if (updatedBook == null)
            {
                _logger.LogWarning("Invalid book data provided for update.");
                return BadRequest("Book details are incorrect.");
            }

            var result = await _mediator.Send(new UpdateBookByIdCommand(updatedBookId, updatedBook));

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully updated book with Id: {BookId}", updatedBookId);
                return Ok(result.Data);
            }

            _logger.LogWarning("Failed to update book with Id: {BookId}. Error: {ErrorMessage}", updatedBookId, result.Message);
            return BadRequest(result.Message);
        }

        // DELETE api/<BookController>/5
        [Authorize]
        [HttpDelete]
        [Route("DeleteBook/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id));

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully deleted book with Id: {BookId}", id);
                return Ok(new { message = $"Successfully deleted book with Id: {id}" });
            }

            _logger.LogWarning("Failed to delete book with Id: {BookId}. Error: {ErrorMessage}", id, result.Message);
            return BadRequest(new { message = result.Message });
        }
    }
}
