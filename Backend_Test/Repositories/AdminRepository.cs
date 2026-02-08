using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace Backend_Test.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly GVar _gVar = new GVar();

        public async Task<Administrator> GetByUsernameAsync(string username)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM Administrators WHERE Username = @Username";
            return await connection.QuerySingleOrDefaultAsync<Administrator>(sql, new { Username = username });
        }

        public async Task<int> CreateAsync(Administrator admin)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = @"
                INSERT INTO Administrators (Username, PasswordHash, Role, CreatedAt)
                VALUES (@Username, @PasswordHash, @Role, @CreatedAt)
                RETURNING Id;";
            return await connection.ExecuteScalarAsync<int>(sql, admin);
        }
    }
}