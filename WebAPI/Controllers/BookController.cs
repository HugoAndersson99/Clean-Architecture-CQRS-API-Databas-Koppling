using Domain;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Commands.Books.AddBook;
using Application.Queries.Books.GetAll;
using Application.Commands.Books.DeleteBook;
using Application.Commands.Books.UpdateBook;
using Application.Queries.Books.GetById;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        internal readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<BookController>
        [Authorize]
        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IActionResult> GetAllBooksFromDB()
        {
            return Ok(await _mediator.Send(new GetAllBooksQuery()));
            
        }

        // GET api/<BookController>/5
        [Authorize]
        [HttpGet]
        [Route("getBookById/{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            return Ok(await _mediator.Send(new GetBookByIdQuery(bookId)));
        }

        // POST api/<BookController>
        [Authorize]
        [HttpPost]
        [Route("AddNewBook")]
        public async void Post([FromBody] Book bookToAdd)
        {
            await _mediator.Send(new AddBookCommand(bookToAdd));
        }

        // PUT api/<BookController>/5
        [Authorize]
        [HttpPut]
        [Route("updateBook/{updatedBookId}")]
        public async Task<IActionResult> UpdateBook([FromBody] Book updatedBook, int updatedBookId)
        {
            return Ok(await _mediator.Send(new UpdateBookByIdCommand(updatedBook, updatedBookId)));
        }

        // DELETE api/<BookController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async void DeleteBook(int id)
        {
            await _mediator.Send(new DeleteBookCommand(id));
        }
    }
}
