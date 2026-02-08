using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;
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
                using var connection = new SqlConnection(oGvar.conn);

                var parameters = new DynamicParameters();

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "Store_Procedure_Name",
                    parameters,
                    commandType: CommandType.StoredProcedure
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
                using var connection = new SqlConnection(oGvar.conn);

                var parameters = new DynamicParameters();

                var result = await connection.QueryFirstOrDefaultAsync<PassengerModel>(
                    "Store_Procedure_Name",
                    parameters,
                    commandType: CommandType.StoredProcedure
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
                using var connection = new SqlConnection(oGvar.conn);

                var parameters = new DynamicParameters();
                parameters.Add("@doc_id", docId);

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "Store_Procedure_Name",
                    parameters,
                    commandType: CommandType.StoredProcedure
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
