using Backend_Test.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country> GetByIdAsync(int id);
    }
}
