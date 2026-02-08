using Backend_Test.Models;

namespace Backend_Test.Repositories.Interfaces
{
    public interface IErrorLogRepository
    {
        Task<bool> AddLogData(ErrorLogModel data);
    }
}
