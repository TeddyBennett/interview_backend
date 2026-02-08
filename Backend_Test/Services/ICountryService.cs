using Backend_Test.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public interface ICountryService
    {
        Task<ApiResponse<IEnumerable<Country>>> GetAllCountriesAsync();
        Task<ApiResponse<Country>> GetCountryByIdAsync(int id);
    }
}
