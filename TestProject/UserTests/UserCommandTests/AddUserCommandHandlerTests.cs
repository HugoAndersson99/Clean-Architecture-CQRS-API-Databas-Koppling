using Application.ApplicationDtos;
using Application.Commands.Users.AddNewUser;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UserTests.UserCommandTests
{
    [TestFixture]
    public class AddUserCommandHandlerTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ILogger<UserRepository>> _mockLogger;
        private AddNewUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<UserRepository>>();

            _handler = new AddNewUserCommandHandler(_mockUserRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenUserAdded()
        {
            // Arrange
            var userDto = new UserDto { UserName = "testuser", Password = "password123" };
            var command = new AddNewUserCommand(userDto);
            var userToAdd = new User { Id = Guid.NewGuid(), UserName = userDto.UserName, Password = userDto.Password };

            _mockUserRepository
                .Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                .ReturnsAsync(OperationResult<User>.Success(userToAdd, "User added successfully."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }
        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAddUserFails()
        {
            // Arrange
            var userDto = new UserDto { UserName = "testuser", Password = "password123" };
            var command = new AddNewUserCommand(userDto);
            var exceptionMessage = "Database error occurred.";

            _mockUserRepository
                .Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                .ReturnsAsync(OperationResult<User>.Failure(exceptionMessage, "Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
            _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }
    }
}
