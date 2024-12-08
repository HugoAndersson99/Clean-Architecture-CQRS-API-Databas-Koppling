

using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.GetAllUsers;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UserTests.UserQueryTests
{
    [TestFixture]
    public class GetAllUsersQueryHandlerTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ILogger<GetAllUsersQueryHandler>> _mockLogger;
        private GetAllUsersQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<GetAllUsersQueryHandler>>();
            _handler = new GetAllUsersQueryHandler(_mockUserRepository.Object, _mockLogger.Object);
        }
        [Test]
        public async Task Handle_ShouldReturnUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), UserName = "user1", Password = "password1" },
            new User { Id = Guid.NewGuid(), UserName = "user2", Password = "password2" }
        };

            _mockUserRepository
                .Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(OperationResult<List<User>>.Success(users));

            // Act
            var result = await _handler.Handle(new GetAllUsersQuery(), CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.Count, Is.EqualTo(users.Count));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenNoUsersExist()
        {
            // Arrange
            var users = new List<User>();

            _mockUserRepository
                .Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(OperationResult<List<User>>.Failure("No users found."));

            // Act
            var result = await _handler.Handle(new GetAllUsersQuery(), CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("No users found."));
        }

    }
}
