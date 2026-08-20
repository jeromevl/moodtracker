using MoodTracker.Core.Interfaces;
using MoodTracker.Core.Interfaces.Repositories;
using MoodTracker.Core.Models;
using MoodTracker.Core.Services;
using Moq;

namespace MoodTracker.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _sut = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User { Id = userId, Username = "Jerome" };

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("Jerome"))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _sut.GetUserByUsernameAsync("Jerome");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Jerome", result.Username);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("Unknown"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _sut.GetUserByUsernameAsync("Unknown");

            // Assert
            Assert.Null(result);
        }
    }
}