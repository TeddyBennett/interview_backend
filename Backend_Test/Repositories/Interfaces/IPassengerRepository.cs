using Backend_Test.Models;
using static Backend_Test.Models.PassengerModel;

namespace Backend_Test.Repositories.Interfaces
{
    public interface IPassengerRepository
    {
        Task<int> AddPassenger(PassengerModel passenger);
        Task<bool> UpdatePassenger(PassengerModel passenger, int passengerId);
        Task<bool> DeletePassenger(int passengerId);
        Task<PassengerModel> GetPassengerById(int passengerId);
        Task<int> GetPassengersDetail(int docId);
    }
}
