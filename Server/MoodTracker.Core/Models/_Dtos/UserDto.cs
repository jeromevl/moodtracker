namespace MoodTracker.Core.Models._Dtos
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

        public User ToModel()
        {
            return new User
            {
                Id = this.Id,
                Username = this.Username,
                Role = this.Role
            };
        }

        public static UserDto FromModel(User model)
        {
            return new UserDto
            {
                Id = model.Id,
                Username = model.Username,
                Role = model.Role
            };
        }
    }
}