using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository; // Injecting User Repository
        private readonly IConfiguration _configuration;

        public AuthService(IAdminRepository adminRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var admin = await _adminRepository.GetByUsernameAsync(username);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                return null;
            }

            return GenerateJwtToken(admin.Id.ToString(), admin.Username, admin.Role);
        }

        public async Task<string> UserLoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return GenerateJwtToken(user.Id, user.Username, user.Role);
        }

        public async Task<Administrator> RegisterAdminAsync(string username, string password)
        {
            var existing = await _adminRepository.GetByUsernameAsync(username);
            if (existing != null) throw new Exception("Username already exists");

            var admin = new Administrator
            {
                Username = username,
                Password = HashPassword(password),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            admin.Id = await _adminRepository.CreateAsync(admin);
            return admin;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private string GenerateJwtToken(string id, string username, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
