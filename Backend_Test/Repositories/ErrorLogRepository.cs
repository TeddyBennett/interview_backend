using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Data;

namespace Backend_Test.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private GVar oGvar = new GVar();

        public async Task<bool> AddLogData(ErrorLogModel data)
        {
            using var connection = new NpgsqlConnection(oGvar.conn);

            var parameters = new DynamicParameters();
            parameters.Add("@action", "INSERT");
            parameters.Add("@timestamp", data.timestamp);
            parameters.Add("@status", data.status);
            parameters.Add("@error", data.error);
            parameters.Add("@message", data.message);
            parameters.Add("@path", data.path);
            parameters.Add("@req_from", data.req_from);

            // PostgreSQL stored procedure call
            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "CALL SP_TB_ERR_LOG_DATA(@action, @timestamp, @status, @error, @message, @path, @req_from)",
                parameters
            );

            return result != null && result.Status == "Success";
        }
    }
}