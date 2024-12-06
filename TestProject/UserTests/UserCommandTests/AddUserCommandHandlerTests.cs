using Application.ApplicationDtos;
using Application.Commands.Users.AddNewUser;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UserTests.UserCommandTests
{
    [TestFixture]
    public class AddUserCommandHandlerTests
    {
        private AddNewUserCommandHandler _handler;
        private IUserRepository _mockUserRepository;
        private ILogger<AddNewUserCommandHandler> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = A.Fake<IUserRepository>();
            _mockLogger = A.Fake<ILogger<AddNewUserCommandHandler>>();
            _handler = new AddNewUserCommandHandler(_mockUserRepository, _mockLogger);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenUserIsAddedSuccessfully()
        {
            // Arrange
            var userDto = new UserDto { UserName = "testuser", Password = "password123" };
            var command = new AddNewUserCommand(userDto);

            var operationResult = OperationResult<User>.Success(
                new User { Id = Guid.NewGuid(), UserName = userDto.UserName, Password = userDto.Password },
                "User added successfully.");

            A.CallTo(() => _mockUserRepository.AddUserAsync(A<User>.Ignored))
                .Returns(Task.FromResult(operationResult));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("User added successfully.", result.Message);
        }

        
        [Test]
        public async Task Handle_ShouldAddUser_WhenValidDataIsProvided()
        {
            // Arrange
            var newUser = new UserDto { UserName = "NewUser", Password = "Password123" };
            var createdUser = new User { Id = Guid.NewGuid(), UserName = "NewUser", Password = "Password123" };

            A.CallTo(() => _mockUserRepository.AddUserAsync(A<User>.Ignored))
                .Returns(Task.FromResult(OperationResult<User>.Success(createdUser, "User added successfully.")));

            var command = new AddNewUserCommand(newUser);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("User added successfully.", result.Message);
            Assert.AreEqual("NewUser", result.Data.UserName);

            A.CallTo(() => _mockUserRepository.AddUserAsync(A<User>.That.Matches(u => u.UserName == "NewUser")))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            // Arrange
            var newUser = new UserDto { UserName = "TestUser", Password = "Password123" };

            // Simulera ett undantag i repositoryn
            A.CallTo(() => _mockUserRepository.AddUserAsync(A<User>.Ignored))
                .Throws(new Exception("Database error."));

            var command = new AddNewUserCommand(newUser);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess); // Det ska vara ett misslyckat resultat
            Assert.AreEqual("Database error.", result.Message); // Kontrollera exakt felmeddelande
        }
    }
}
