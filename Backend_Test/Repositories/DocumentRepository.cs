using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace Backend_Test.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly GVar _gVar = new GVar();

        public async Task<string> CreateAsync(Document document)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = @"
                INSERT INTO Documents (Id, DocumentTypeId, DocumentNumber, IssuedCountryId, IssueDate, ExpiryDate, CreatedAt)
                VALUES (@Id, @DocumentTypeId, @DocumentNumber, @IssuedCountryId, @IssueDate, @ExpiryDate, @CreatedAt)
                RETURNING Id;";
            return await connection.ExecuteScalarAsync<string>(sql, document);
        }

        public async Task<Document> GetByIdAsync(string id)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM Documents WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Document>(sql, new { Id = id });
        }

        public async Task<bool> UpdateAsync(Document document)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = @"
                UPDATE Documents 
                SET DocumentTypeId = @DocumentTypeId, 
                    DocumentNumber = @DocumentNumber, 
                    IssuedCountryId = @IssuedCountryId, 
                    IssueDate = @IssueDate, 
                    ExpiryDate = @ExpiryDate 
                WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, document);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "DELETE FROM Documents WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
