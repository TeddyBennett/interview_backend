using Backend_Test.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories.Interfaces
{
    public interface IApiKeyRepository
    {
        Task<int> CreateAsync(ApiKey apiKey);
        Task<ApiKey> GetByIdAsync(int id);
        Task<ApiKey> GetByHashedKeyAsync(string hashedKey);
        Task<IEnumerable<ApiKey>> GetAllAsync();
        Task<bool> UpdateStatusAsync(int id, bool isEnabled);
        Task<bool> DeleteAsync(int id);
    }
}
