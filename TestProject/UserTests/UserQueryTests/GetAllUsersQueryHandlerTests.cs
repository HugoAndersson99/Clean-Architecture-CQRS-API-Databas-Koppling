

using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.GetAllUsers;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UserTests.UserQueryTests
{
    [TestFixture]
    public class GetAllUsersQueryHandlerTests
    {
        private IUserRepository _mockUserRepository;
        private ILogger<GetAllUsersQueryHandler> _mockLogger;
        private GetAllUsersQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = A.Fake<IUserRepository>();
            _mockLogger = A.Fake<ILogger<GetAllUsersQueryHandler>>();
            var _mochCache = A.Fake<IMemoryCache>();
            _handler = new GetAllUsersQueryHandler(_mockUserRepository, _mockLogger, _mochCache);
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

            // Förbered så att GetAllUsersAsync returnerar användare
            A.CallTo(() => _mockUserRepository.GetAllUsersAsync())
                .Returns(OperationResult<List<User>>.Success(users));

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

            // Förbered så att GetAllUsersAsync returnerar ett misslyckande
            A.CallTo(() => _mockUserRepository.GetAllUsersAsync())
                .Returns(OperationResult<List<User>>.Failure("No users found or error occurred."));

            // Act
            var result = await _handler.Handle(new GetAllUsersQuery(), CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("No users found or error occurred."));
        }

    }
}
