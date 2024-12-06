using Application.ApplicationDtos;
using Application.Commands.Users.AddNewUser;
using Application.Queries.Users.GetAllUsers;
using Application.Queries.Users.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.UsersController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());

            if (result.IsSuccess)
            {
                return Ok(result.Data); // Return the list of all users.
            }

            _logger.LogWarning("Failed to fetch users. Error: {ErrorMessage}", result.Message);
            return BadRequest(result.Message); // Return BadRequest if fetching users fails.
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto newUser)
        {
            if (newUser == null || string.IsNullOrWhiteSpace(newUser.UserName) || string.IsNullOrWhiteSpace(newUser.Password))
            {
                _logger.LogWarning("Registration attempt failed: missing user credentials.");
                return BadRequest("User credentials are required.");
            }

            var result = await _mediator.Send(new AddNewUserCommand(newUser));

            if (result.IsSuccess)
            {
                _logger.LogInformation("User {UserName} registered successfully.", newUser.UserName);
                return CreatedAtAction(nameof(GetAllUsers), result.Data); // Redirect to GetAllUsers after successful registration
            }

            _logger.LogWarning("Registration failed for user: {UserName}. Error: {ErrorMessage}", newUser.UserName, result.Message);
            return BadRequest(result.Message); // Return BadRequest if registration fails.
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDto loginUser)
        {
            if (loginUser == null || string.IsNullOrWhiteSpace(loginUser.UserName) || string.IsNullOrWhiteSpace(loginUser.Password))
            {
                _logger.LogWarning("Login attempt failed: missing user credentials.");
                return BadRequest("User credentials are required.");
            }

            var result = await _mediator.Send(new LoginUserQuery(loginUser));

            if (result.IsSuccess)
            {
                _logger.LogInformation("User {UserName} logged in successfully.", loginUser.UserName);
                return Ok(result.Data); // The result should be a JWT token or success message.
            }

            _logger.LogWarning("Login failed for user: {UserName}. Error: {ErrorMessage}", loginUser.UserName, result.Message);
            return Unauthorized(result.Message); // Return Unauthorized if login fails.
        }
        
    }
}
