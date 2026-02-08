using Backend_Test.Helpers;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;

namespace Backend_Test.Services
{
    public class ErrorLogService
    {
        private readonly IErrorLogRepository _repository;

        public ErrorLogService(IErrorLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> SaveLogData(ErrorLogModel data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return await _repository.AddLogData(data);
        }
    }
}
