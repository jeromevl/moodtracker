using MoodTracker.Core.Interfaces.Repositories;
using MoodTracker.Core.Models;
using MoodTracker.Core.Services;
using Moq;

namespace MoodTracker.Tests
{
    public class MoodServiceTests
    {
        private readonly Mock<IMoodRepository> _moodRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly MoodService _sut;

        public MoodServiceTests()
        {
            _moodRepositoryMock = new Mock<IMoodRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _sut = new MoodService(_moodRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetMoodsByUserAsync_ShouldReturnMappedDtos_WhenUserMoodsExist()
        {
            // Arrange
            var username = "Jerome";
            var fakeMoods = new List<UserMood>
            {
                new() { Date = DateOnly.FromDateTime(DateTime.UtcNow), Mood = new Mood { Id = 4, Name = "Feeling great" } }
            };

            _moodRepositoryMock.Setup(r => r.GetAllByUserAsync(username))
                .ReturnsAsync(fakeMoods);

            // Act
            var result = await _sut.GetMoodsByUserAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _moodRepositoryMock.Verify(r => r.GetAllByUserAsync(username), Times.Once);
        }

        [Fact]
        public async Task SubmitMoodByUsernameAsync_ShouldCreateUserAndSubmitMood_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "NewUser";
            var moodId = 2;
            var date = new DateOnly(2026, 8, 19);

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(username))
                .ReturnsAsync((User?)null);

            _moodRepositoryMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>(), date))
                .ReturnsAsync(false);

            // Act
            await _sut.SubmitMoodByUsernameAsync(username, moodId, date);

            // Assert
            _userRepositoryMock.Verify(r => r.CreateAsync(It.Is<User>(u => u.Username == username), null), Times.Once);
            _moodRepositoryMock.Verify(r => r.SubmitAsync(It.IsAny<Guid>(), moodId, date, null), Times.Once);
        }

        [Fact]
        public async Task SubmitMoodByUsernameAsync_ShouldNotCreateUser_WhenUserAlreadyExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User { Id = userId, Username = "ExistingUser" };
            var date = new DateOnly(2026, 8, 19);

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(existingUser.Username))
                .ReturnsAsync(existingUser);

            _moodRepositoryMock.Setup(r => r.ExistsAsync(userId, date))
                .ReturnsAsync(false);

            // Act
            await _sut.SubmitMoodByUsernameAsync(existingUser.Username, 1, date);

            // Assert
            _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>(), null), Times.Never);
            _moodRepositoryMock.Verify(r => r.SubmitAsync(userId, 1, date, null), Times.Once);
        }

        [Fact]
        public async Task SubmitMoodByUsernameAsync_ShouldThrowInvalidOperationException_WhenMoodAlreadyExistsForDate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User { Id = userId, Username = "Jerome" };
            var date = new DateOnly(2026, 8, 19);

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(existingUser.Username))
                .ReturnsAsync(existingUser);

            _moodRepositoryMock.Setup(r => r.ExistsAsync(userId, date))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _sut.SubmitMoodByUsernameAsync(existingUser.Username, 1, date));

            Assert.Equal("Mood entry for the specified user and date already exists.", exception.Message);
            _moodRepositoryMock.Verify(r => r.SubmitAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<DateOnly>(), null), Times.Never);
        }
    }
}