using Backend_Test.Models;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task<string> UserLoginAsync(string username, string password); // New login for Users
        Task<Administrator> RegisterAdminAsync(string username, string password);
        string HashPassword(string password);
    }
}
