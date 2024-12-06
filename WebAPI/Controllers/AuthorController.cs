using Application.ApplicationDtos;
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
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IMediator mediator, ILogger<AuthorController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/<AuthorController>
        [Authorize]
        [HttpGet]
        [Route("getAllAuthors")]
        public async Task<IActionResult> GetAllAuthorsFromDB()
        {
            var operationResult = await _mediator.Send(new GetAllAuthorsQuery());

            if (operationResult.IsSuccess)
            {
                var authorsWithBooks = operationResult.Data.Select(author => new
                {
                    author.Id,
                    author.Name,
                    Books = author.Books
                });

                return Ok(authorsWithBooks);
            }

            _logger.LogWarning("Failed to retrieve all authors. Error: {ErrorMessage}", operationResult.Message);
            return BadRequest(operationResult.Message);
        }

        // GET api/<AuthorController>/5
        [Authorize]
        [HttpGet]
        [Route("getAuthorById/{authorId}")]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            var operationResult = await _mediator.Send(new GetAuthorByIdQuery(authorId));

            if (operationResult.IsSuccess)
            {
                var authorWithBooks = new
                {
                    operationResult.Data.Id,
                    operationResult.Data.Name,
                    Books = operationResult.Data.Books
                };

                return Ok(authorWithBooks);
            }

            _logger.LogWarning("Failed to retrieve author with Id: {AuthorId}. Error: {ErrorMessage}", authorId, operationResult.Message);
            return NotFound(operationResult.Message);
        }

        // POST api/<AuthorController>
        [Authorize]
        [HttpPost]
        [Route("AddNewAuthor")]
        public async Task<IActionResult> Post([FromBody] AuthorDto authorToAdd)
        {
            if (authorToAdd == null)
            {
                _logger.LogWarning("Author details were not provided.");
                return BadRequest("Author details must be provided.");
            }

            var result = await _mediator.Send(new AddAuthorCommand(authorToAdd));

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully added new author with Id: {AuthorId}", result.Data.Id);
                return CreatedAtAction(nameof(GetAuthorById), new { authorId = result.Data.Id }, result.Data);
            }

            _logger.LogWarning("Failed to add author: {ErrorMessage}", result.Message);
            return BadRequest(result.Message);
        }

        // PUT api/<AuthorController>/5
        [Authorize]
        [HttpPut]
        [Route("updateAuthor/{updatedAuthorId}")]
        public async Task<IActionResult> UpdateAuthor([FromBody] AuthorDto updatedAuthor, int updatedAuthorId)
        {
            if (updatedAuthor == null)
            {
                _logger.LogWarning("Invalid author data provided for update.");
                return BadRequest("Author details are incorrect.");
            }

            var result = await _mediator.Send(new UpdateAuthorByIdCommand(updatedAuthor, updatedAuthorId));

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully updated author with Id: {AuthorId}", updatedAuthorId);
                return Ok(result.Data);
            }

            _logger.LogWarning("Failed to update author with Id: {AuthorId}. Error: {ErrorMessage}", updatedAuthorId, result.Message);
            return BadRequest(result.Message);
        }

        // DELETE api/<AuthorController>/5
        [Authorize]
        [HttpDelete]
        [Route("DeleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await _mediator.Send(new DeleteAuthorCommand(id));

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully deleted author with Id: {AuthorId}", id);
                return Ok(new { message = $"Successfully deleted author with Id: {id}" });
            }

            _logger.LogWarning("Failed to delete author with Id: {AuthorId}. Error: {ErrorMessage}", id, result.Message);
            return BadRequest(new { message = result.Message });
        }
    }
}
