using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly GVar _gVar = new GVar();

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT Id, Code, Name FROM Countries ORDER BY Name";
            return await connection.QueryAsync<Country>(sql);
        }

        public async Task<Country> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT Id, Code, Name FROM Countries WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Country>(sql, new { Id = id });
        }
    }
}
