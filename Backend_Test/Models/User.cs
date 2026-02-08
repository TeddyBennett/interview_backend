using System;

namespace Backend_Test.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserCreateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }

    public class UserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
