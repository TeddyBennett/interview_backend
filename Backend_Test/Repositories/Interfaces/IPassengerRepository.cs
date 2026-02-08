using Backend_Test.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories.Interfaces
{
    public interface IPassengerRepository
    {
        Task<int> CreateAsync(PassengerModel passenger);
        Task<bool> UpdateAsync(PassengerModel passenger); // Update by ID within the model
        Task<bool> DeleteAsync(int passengerId);
        Task<PassengerModel> GetByIdAsync(int passengerId);
        Task<IEnumerable<PassengerModel>> GetAllAsync(); // Add GetAll for consistency
        Task<int> GetPassengersDetail(int docId); // Keep as is for now
    }
}
