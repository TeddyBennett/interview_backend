using API_DVETS.Services;
using Backend_Test.Exceptions;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using static Backend_Test.Models.PassengerModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend_Test.Services
{
    public class PassengerService
    {
        private readonly IPassengerRepository _repository;
        private readonly ILogger<PassengerService> _logger;
        private readonly MinioService _minioService;

        public PassengerService(IPassengerRepository repository, ILogger<PassengerService> logger, MinioService minioService)
        {
            _repository = repository;
            _logger = logger;
            _minioService = minioService;
        }

        public async Task<ApiResponse<int>> AddPassenger(PassengerModel passenger)
        {

            _logger.LogInformation("Passenger added successfully with ID: {PassengerId}", 0);
            return ApiResponse<int>.Created(0, "Passenger added successfully");
        }


        public async Task<ApiResponse<bool>> UpdatePassenger(PassengerModel passenger, int passengerId)
        {
            try
            {

                return ApiResponse<bool>.Ok(true, "Passenger updated successfully");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error when updating passenger");
                return ApiResponse<bool>.Fail(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Passenger not found when updating");
                return ApiResponse<bool>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating passenger");
                return ApiResponse<bool>.Fail("An error occurred while updating the passenger");
            }
        }

        public async Task<ApiResponse<bool>> DeletePassenger(int passengerId)
        {
            try
            {
                if (passengerId <= 0)
                    throw new ValidationException("Passenger ID is required");

                var existingPassenger = await _repository.GetPassengerById(passengerId);
                if (existingPassenger == null)
                    throw new NotFoundException($"Passenger with ID {passengerId} not found");

                var result = await _repository.DeletePassenger(passengerId);
                _logger.LogInformation("Passenger deleted successfully. ID: {PassengerId}", passengerId);

                return ApiResponse<bool>.Ok(result, "Passenger deleted successfully");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error when deleting passenger");
                return ApiResponse<bool>.Fail(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Passenger not found when deleting");
                return ApiResponse<bool>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting passenger");
                return ApiResponse<bool>.Fail("An error occurred while deleting the passenger");
            }
        }

        public async Task<ApiResponse<PassengerModel>> GetPassengerById(int passengerId)
        {
            try
            {
                if (passengerId <= 0)
                    throw new ValidationException("Passenger ID is required");

                var passenger = await _repository.GetPassengerById(passengerId);

                if (passenger == null)
                    throw new NotFoundException($"Passenger with ID {passengerId} not found");

                return ApiResponse<PassengerModel>.Ok(passenger, "Passenger retrieved successfully");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error when getting passenger");
                return ApiResponse<PassengerModel>.Fail(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Passenger not found");
                return ApiResponse<PassengerModel>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting passenger by ID {PassengerId}", passengerId);
                return ApiResponse<PassengerModel>.Fail("An error occurred while retrieving the passenger");
            }
        }

    }
}
