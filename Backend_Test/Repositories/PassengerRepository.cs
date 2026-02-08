using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Data;
using static Backend_Test.Models.PassengerModel;

namespace Backend_Test.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly ILogger<PassengerRepository> _logger;
        private GVar oGvar = new GVar();

        public PassengerRepository(IConfiguration configuration, ILogger<PassengerRepository> logger)
        {
            _logger = logger;
        }

        public async Task<int> AddPassenger(PassengerModel passenger)
        {
            return 0;
        }

        public async Task<bool> UpdatePassenger(PassengerModel passenger, int passengerId)
        {
            return true;
        }

        public async Task<bool> DeletePassenger(int passengerId)
        {
            try
            {
                using var connection = new NpgsqlConnection(oGvar.conn);

                var parameters = new DynamicParameters();

                // Note: PostgreSQL calls procedures/functions differently. 
                // This will likely need updating based on your actual procedure names.
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "CALL Store_Procedure_Name(@params)", // Placeholder syntax for PG
                    parameters
                );

                if (result == null || result.Status != "Success")
                {
                    throw new Exception(result?.Message ?? "Failed to delete passenger");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting passenger");
                throw new Exception(ex.Message);
            }
        }

        public async Task<PassengerModel> GetPassengerById(int passengerId)
        {
            try
            {
                using var connection = new NpgsqlConnection(oGvar.conn);

                var parameters = new DynamicParameters();

                var result = await connection.QueryFirstOrDefaultAsync<PassengerModel>(
                    "SELECT * FROM Store_Procedure_Name(@params)", // Placeholder for PG function
                    parameters
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting passenger by ID {PassengerId}", passengerId);
                throw new Exception(ex.Message);
            }
        }


        public async Task<int> GetPassengersDetail(int docId)
        {
            try
            {
                using var connection = new NpgsqlConnection(oGvar.conn);

                var parameters = new DynamicParameters();
                parameters.Add("@doc_id", docId);

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "SELECT * FROM Store_Procedure_Name(@doc_id)",
                    parameters
                );

                if (result == null)
                    return 0;

                return result.passenger_count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting passenger detail by document ID {DocId}", docId);
                throw new Exception(ex.Message);
            }
        }
    }
}