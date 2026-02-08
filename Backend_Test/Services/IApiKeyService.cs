using Backend_Test.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public interface IApiKeyService
    {
        Task<string> CreateApiKeyAsync(string clientName, int createdById, DateTime? expiresAt);
        Task<IEnumerable<ApiKey>> GetAllApiKeysAsync();
        Task<ApiKey> GetApiKeyByIdAsync(int id);
        Task<bool> ValidateApiKeyAsync(string rawApiKey);
        Task<bool> ToggleApiKeyStatusAsync(int id, bool isEnabled);
        Task<bool> DeleteApiKeyAsync(int id);
    }
}
