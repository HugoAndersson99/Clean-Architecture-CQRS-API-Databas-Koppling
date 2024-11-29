using Application.Commands.Authors.AddAuthor;
using Application.Commands.Authors.DeleteAuthor;
using Application.Commands.Authors.UpdateAuthor;
using Application.Queries.Authors.GetAllAuthors;
using Application.Queries.Authors.GetAuthorById;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        internal readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<AuthorController>
        [Authorize]
        [HttpGet]
        [Route("getAllAuthors")]
        public async Task<IActionResult> GetAllAuthorsFromDB()
        {
            // return Ok(await _mediator.Send(new GetAllAuthorsQuery()));
            var authors = await _mediator.Send(new GetAllAuthorsQuery());

            var authorsWithBooks = authors.Select(author => new
            {
                author.Id,
                author.Name,
                Books = author.Books 
            });

            return Ok(authorsWithBooks);
        }

        // GET api/<AuthorController>/5
        [Authorize]
        [HttpGet]
        [Route("getAuthorById/{authorId}")]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(authorId));

            var authorWithBooks = new
            {
                author.Id,
                author.Name,
                Books = author.Books
            };

            return Ok(authorWithBooks);
        }

        // POST api/<AuthorController>
        [Authorize]
        [HttpPost]
        [Route("AddNewAuthor")]
        public async void Post([FromBody] Author authorToAdd)
        {
            await _mediator.Send(new AddAuthorCommand(authorToAdd));
        }

        // PUT api/<AuthorController>/5
        [Authorize]
        [HttpPut]
        [Route("updateAuthor/{updatedAuthorId}")]
        public async Task<IActionResult> UpdateAuthor([FromBody] Author updatedAuthor, int updatedAuthorId)
        {
            return Ok(await _mediator.Send(new UpdateAuthorByIdCommand(updatedAuthor, updatedAuthorId)));
        }

        // DELETE api/<AuthorController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async void DeleteAuthor(int id)
        {
            await _mediator.Send(new DeleteAuthorCommand(id));
        }
    }
}
