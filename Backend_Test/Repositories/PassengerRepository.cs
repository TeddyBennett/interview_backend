using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly ILogger<PassengerRepository> _logger;
        private readonly GVar _gVar = new GVar();

        public PassengerRepository(ILogger<PassengerRepository> logger)
        {
            _logger = logger;
        }

        public async Task<int> CreateAsync(PassengerModel passenger)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = @"
                INSERT INTO Passengers (DocId, FirstName, LastName, DateOfBirth, Gender, FaceImageUrl, CreatedByUserId, CreatedAt)
                VALUES (@DocId, @FirstName, @LastName, @DateOfBirth, @Gender, @FaceImageUrl, @CreatedByUserId, @CreatedAt)
                RETURNING PassengerId;";
            return await connection.ExecuteScalarAsync<int>(sql, passenger);
        }

        public async Task<bool> UpdateAsync(PassengerModel passenger)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = @"
                UPDATE Passengers 
                SET DocId = @DocId,
                    FirstName = @FirstName, 
                    LastName = @LastName, 
                    DateOfBirth = @DateOfBirth,
                    Gender = @Gender,
                    FaceImageUrl = @FaceImageUrl,
                    CreatedByUserId = @CreatedByUserId
                WHERE PassengerId = @PassengerId;";
            var rowsAffected = await connection.ExecuteAsync(sql, passenger);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int passengerId)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "DELETE FROM Passengers WHERE PassengerId = @PassengerId;";
            var rowsAffected = await connection.ExecuteAsync(sql, new { PassengerId = passengerId });
            return rowsAffected > 0;
        }

        public async Task<PassengerModel> GetByIdAsync(int passengerId)
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM Passengers WHERE PassengerId = @PassengerId;";
            return await connection.QuerySingleOrDefaultAsync<PassengerModel>(sql, new { PassengerId = passengerId });
        }
        
        public async Task<IEnumerable<PassengerModel>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_gVar.conn);
            const string sql = "SELECT * FROM Passengers ORDER BY PassengerId DESC;";
            return await connection.QueryAsync<PassengerModel>(sql);
        }

        public async Task<int> GetPassengersDetail(int docId)
        {
            // This might need update if docId is now string, but for now keeping it as is 
            // since the interface says int docId.
            return 0; 
        }
    }
}