using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<ApiResponse<IEnumerable<Country>>> GetAllCountriesAsync()
        {
            var countries = await _countryRepository.GetAllAsync();
            return ApiResponse<IEnumerable<Country>>.Ok(countries);
        }

        public async Task<ApiResponse<Country>> GetCountryByIdAsync(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            if (country == null)
            {
                return ApiResponse<Country>.Fail("Country not found", 404);
            }
            return ApiResponse<Country>.Ok(country);
        }
    }
}
