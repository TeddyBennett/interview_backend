using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GVar _gVar = new GVar();

        public async Task<string> CreateAsync(User user)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = @"
                INSERT INTO Users (Id, Username, Password, FullName, ProfileImageUrl, Role, CreatedAt)
                VALUES (@Id, @Username, @Password, @FullName, @ProfileImageUrl, @Role, @CreatedAt)
                RETURNING Id;";
            return await connection.ExecuteScalarAsync<string>(sql, user);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM Users WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM Users WHERE Username = @Username";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM Users ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = @"
                UPDATE Users 
                SET Username = @Username, 
                    FullName = @FullName, 
                    ProfileImageUrl = @ProfileImageUrl,
                    Role = @Role 
                WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, user);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "DELETE FROM Users WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
