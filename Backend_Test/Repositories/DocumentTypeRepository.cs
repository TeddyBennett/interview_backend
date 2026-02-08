using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly GVar _gVar = new GVar();

        public async Task<IEnumerable<DocumentType>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT Id, Name FROM DocumentTypes ORDER BY Name";
            return await connection.QueryAsync<DocumentType>(sql);
        }

        public async Task<DocumentType> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT Id, Name FROM DocumentTypes WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<DocumentType>(sql, new { Id = id });
        }
    }
}
