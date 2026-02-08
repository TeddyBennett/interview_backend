using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly GVar _gVar = new GVar();

        public async Task<int> CreateAsync(ApiKey apiKey)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = @"
                INSERT INTO ApiKeys (ClientName, HashedKey, IsEnabled, CreatedById, CreatedAt, ExpiresAt)
                VALUES (@ClientName, @HashedKey, @IsEnabled, @CreatedById, @CreatedAt, @ExpiresAt)
                RETURNING Id;";
            return await connection.ExecuteScalarAsync<int>(sql, apiKey);
        }

        public async Task<ApiKey> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM ApiKeys WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<ApiKey>(sql, new { Id = id });
        }

        public async Task<ApiKey> GetByHashedKeyAsync(string hashedKey)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM ApiKeys WHERE HashedKey = @HashedKey";
            return await connection.QuerySingleOrDefaultAsync<ApiKey>(sql, new { HashedKey = hashedKey });
        }

        public async Task<IEnumerable<ApiKey>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM ApiKeys ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<ApiKey>(sql);
        }

        public async Task<bool> UpdateStatusAsync(int id, bool isEnabled)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "UPDATE ApiKeys SET IsEnabled = @IsEnabled WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, IsEnabled = isEnabled });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "DELETE FROM ApiKeys WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}