
using Application.ApplicationDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.LoginUser;
using Application.Queries.Users.LoginUser.Helpers;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UserTests.UserQueryTests
{
   // [TestFixture]
   // public class LoginUserQueryHandlerTests
   // {
   //     private Mock<IUserRepository> _mockUserRepository;
   //     private Mock<ILogger<LoginUserQueryHandler>> _mockLogger;
   //     private LoginUserQueryHandler _handler;
   //     private readonly TokenHelper _tokenHelper;
   //     [SetUp]
   //     public void Setup()
   //     {
   //         _mockUserRepository = new Mock<IUserRepository>();
   //         _mockLogger = new Mock<ILogger<LoginUserQueryHandler>>();
   //         _handler = new LoginUserQueryHandler(_mockUserRepository.Object, _mockLogger.Object);
   //         _tokenHelper = new TokenHelper();
   //     }
   //
   //     [Test]
   //     public async Task Handle_ShouldReturnSuccess_WhenLoginIsSuccessful()
   //     {
   //         // Arrange
   //         var user = new User { Id = Guid.NewGuid(), UserName = "testUser", Password = "password123" };
   //         _mockUserRepository.Setup(repo => repo.LoginUserAsync("testUser", "password123"))
   //                            .ReturnsAsync(OperationResult<User>.Success(user, "Login successful."));
   //
   //         var command = new LoginUserQuery(new UserDto { UserName = "testUser", Password = "password123" });
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.That(result.IsSuccess, Is.True);
   //         Assert.That(result.Data.UserName, Is.EqualTo("testUser"));
   //     }
   //
   //     [Test]
   //     public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
   //     {
   //         // Arrange
   //         _mockUserRepository.Setup(repo => repo.LoginUserAsync("nonExistentUser", "password123"))
   //                            .ReturnsAsync(OperationResult<User>.Failure("Invalid username or password."));
   //
   //         var command = new LoginUserQuery(new UserDto { UserName = "nonExistentUser", Password = "password123" });
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.That(result.IsSuccess, Is.False);
   //         Assert.That(result.ErrorMessage, Is.EqualTo("Invalid username or password."));
   //     }
   //
   //     [Test]
   //     public async Task Handle_ShouldReturnFailure_WhenPasswordIsIncorrect()
   //     {
   //         // Arrange
   //         var user = new User { Id = Guid.NewGuid(), UserName = "testUser", Password = "password123" };
   //         _mockUserRepository.Setup(repo => repo.LoginUserAsync("testUser", "wrongPassword"))
   //                            .ReturnsAsync(OperationResult<User>.Failure("Invalid username or password."));
   //
   //         var command = new LoginUserQuery(new UserDto { UserName = "testUser", Password = "wrongPassword" });
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.That(result.IsSuccess, Is.False);
   //         Assert.That(result.ErrorMessage, Is.EqualTo("Invalid username or password."));
   //     }
   //
   //     [Test]
   //     public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
   //     {
   //         // Arrange
   //         _mockUserRepository.Setup(repo => repo.LoginUserAsync("testUser", "password123"))
   //                            .ThrowsAsync(new Exception("Database error"));
   //
   //         var command = new LoginUserQuery(new UserDto { UserName = "testUser", Password = "password123" });
   //
   //         // Act
   //         var result = await _handler.Handle(command, CancellationToken.None);
   //
   //         // Assert
   //         Assert.That(result.IsSuccess, Is.False);
   //         Assert.That(result.ErrorMessage, Is.EqualTo("An error occurred: Database error"));
   //     }
   // }
}
