using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IApiKeyRepository _apiKeyRepository;

        public ApiKeyService(IApiKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }

        public async Task<string> CreateApiKeyAsync(string clientName, int createdById, DateTime? expiresAt)
        {
            var rawKeyBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(rawKeyBytes);
            }
            var rawKey = BitConverter.ToString(rawKeyBytes).Replace("-", "").ToLower();

            var hashedKey = HashApiKey(rawKey);

            var apiKey = new ApiKey
            {
                ClientName = clientName,
                HashedKey = hashedKey,
                IsEnabled = true,
                CreatedById = createdById,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt
            };

            await _apiKeyRepository.CreateAsync(apiKey);

            return rawKey;
        }

        public async Task<bool> ValidateApiKeyAsync(string rawApiKey)
        {
            if (string.IsNullOrWhiteSpace(rawApiKey)) return false;

            var hashedKey = HashApiKey(rawApiKey);
            var apiKeyRecord = await _apiKeyRepository.GetByHashedKeyAsync(hashedKey);

            if (apiKeyRecord == null) return false;
            if (!apiKeyRecord.IsEnabled) return false;
            if (apiKeyRecord.ExpiresAt.HasValue && apiKeyRecord.ExpiresAt.Value < DateTime.UtcNow) return false;

            return true;
        }

        public async Task<IEnumerable<ApiKey>> GetAllApiKeysAsync()
        {
            return await _apiKeyRepository.GetAllAsync();
        }

        public async Task<ApiKey> GetApiKeyByIdAsync(int id)
        {
            return await _apiKeyRepository.GetByIdAsync(id);
        }

        public async Task<bool> ToggleApiKeyStatusAsync(int id, bool isEnabled)
        {
            return await _apiKeyRepository.UpdateStatusAsync(id, isEnabled);
        }

        public async Task<bool> DeleteApiKeyAsync(int id)
        {
            return await _apiKeyRepository.DeleteAsync(id);
        }

        private static string HashApiKey(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
