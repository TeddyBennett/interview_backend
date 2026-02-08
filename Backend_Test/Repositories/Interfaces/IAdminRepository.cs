using Backend_Test.Models;
using System.Threading.Tasks;

namespace Backend_Test.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<Administrator> GetByUsernameAsync(string username);
        Task<int> CreateAsync(Administrator admin);
    }
}
