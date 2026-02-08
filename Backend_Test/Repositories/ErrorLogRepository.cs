using AspStudio.Common;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Backend_Test.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private GVar oGvar = new GVar();

        public async Task<bool> AddLogData(ErrorLogModel data)
        {
            using var connection = new SqlConnection(oGvar.conn);

            var parameters = new DynamicParameters();
            parameters.Add("@action", "INSERT");
            parameters.Add("@timestamp", data.timestamp);
            parameters.Add("@status", data.status);
            parameters.Add("@error", data.error);
            parameters.Add("@message", data.message);
            parameters.Add("@path", data.path);
            parameters.Add("@req_from", data.req_from);

            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SP_TB_ERR_LOG_DATA",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result != null && result.Status == "Success";
        }
    }
}
