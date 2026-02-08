using API_DVETS.Services; // Namespace for MinioService
using Backend_Test.Exceptions;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public class PassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly ILogger<PassengerService> _logger;
        private readonly MinioService _minioService;

        public PassengerService(IPassengerRepository passengerRepository, ILogger<PassengerService> logger, MinioService minioService)
        {
            _passengerRepository = passengerRepository;
            _logger = logger;
            _minioService = minioService;
        }

        public async Task<ApiResponse<int>> CreatePassengerAsync(PassengerModel passenger)
        {
            try
            {
                string? faceImageUrl = null;
                if (passenger.FaceImageFile != null)
                {
                    faceImageUrl = await _minioService.UploadFileAsync(passenger.FaceImageFile, "passengers");
                }
                passenger.FaceImageUrl = faceImageUrl;
                passenger.CreatedAt = DateTime.UtcNow; // Set creation timestamp

                int newPassengerId = await _passengerRepository.CreateAsync(passenger);
                _logger.LogInformation("Passenger added successfully with ID: {PassengerId}", newPassengerId);
                return ApiResponse<int>.Created(newPassengerId, "Passenger added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating passenger");
                return ApiResponse<int>.Fail("An error occurred while creating the passenger");
            }
        }

        public async Task<ApiResponse<bool>> UpdatePassengerAsync(PassengerModel passenger)
        {
            try
            {
                if (passenger.PassengerId <= 0)
                    throw new ValidationException("Passenger ID is required for update");

                var existingPassenger = await _passengerRepository.GetByIdAsync(passenger.PassengerId);
                if (existingPassenger == null)
                    throw new NotFoundException($"Passenger with ID {passenger.PassengerId} not found");

                // Handle image upload/update
                if (passenger.FaceImageFile != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingPassenger.FaceImageUrl))
                    {
                        try { await _minioService.DeleteFileByUrlAsync(existingPassenger.FaceImageUrl); }
                        catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete old passenger image: {Url}", existingPassenger.FaceImageUrl); /* Log and ignore if deletion fails */ }
                    }
                    passenger.FaceImageUrl = await _minioService.UploadFileAsync(passenger.FaceImageFile, "passengers");
                }
                else
                {
                    // If no new file is provided, keep the existing image URL
                    passenger.FaceImageUrl = existingPassenger.FaceImageUrl;
                }

                bool result = await _passengerRepository.UpdateAsync(passenger);
                _logger.LogInformation("Passenger updated successfully. ID: {PassengerId}", passenger.PassengerId);
                return ApiResponse<bool>.Ok(result, "Passenger updated successfully");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error when updating passenger: {Message}", ex.Message);
                return ApiResponse<bool>.Fail(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Passenger not found when updating: {Message}", ex.Message);
                return ApiResponse<bool>.Fail(ex.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating passenger with ID: {PassengerId}", passenger.PassengerId);
                return ApiResponse<bool>.Fail("An error occurred while updating the passenger");
            }
        }

        public async Task<ApiResponse<bool>> DeletePassengerAsync(int passengerId)
        {
            try
            {
                if (passengerId <= 0)
                    throw new ValidationException("Passenger ID is required");

                var existingPassenger = await _passengerRepository.GetByIdAsync(passengerId);
                if (existingPassenger == null)
                    throw new NotFoundException($"Passenger with ID {passengerId} not found");

                // Delete image from MinIO
                if (!string.IsNullOrEmpty(existingPassenger.FaceImageUrl))
                {
                    try { await _minioService.DeleteFileByUrlAsync(existingPassenger.FaceImageUrl); }
                    catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete passenger image during deletion: {Url}", existingPassenger.FaceImageUrl); /* Log and ignore */ }
                }

                bool result = await _passengerRepository.DeleteAsync(passengerId);
                _logger.LogInformation("Passenger deleted successfully. ID: {PassengerId}", passengerId);
                return ApiResponse<bool>.Ok(result, "Passenger deleted successfully");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error when deleting passenger: {Message}", ex.Message);
                return ApiResponse<bool>.Fail(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Passenger not found when deleting: {Message}", ex.Message);
                return ApiResponse<bool>.Fail(ex.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting passenger with ID: {PassengerId}", passengerId);
                return ApiResponse<bool>.Fail("An error occurred while deleting the passenger");
            }
        }

        public async Task<ApiResponse<PassengerModel>> GetPassengerByIdAsync(int passengerId)
        {
            try
            {
                if (passengerId <= 0)
                    throw new ValidationException("Passenger ID is required");

                var passenger = await _passengerRepository.GetByIdAsync(passengerId);

                if (passenger == null)
                    throw new NotFoundException($"Passenger with ID {passengerId} not found");

                return ApiResponse<PassengerModel>.Ok(passenger, "Passenger retrieved successfully");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error when getting passenger: {Message}", ex.Message);
                return ApiResponse<PassengerModel>.Fail(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Passenger not found: {Message}", ex.Message);
                return ApiResponse<PassengerModel>.Fail(ex.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting passenger by ID {PassengerId}", passengerId);
                return ApiResponse<PassengerModel>.Fail("An error occurred while retrieving the passenger");
            }
        }

        public async Task<ApiResponse<IEnumerable<PassengerModel>>> GetAllPassengersAsync()
        {
            try
            {
                var passengers = await _passengerRepository.GetAllAsync();
                return ApiResponse<IEnumerable<PassengerModel>>.Ok(passengers, "Passengers retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all passengers");
                return ApiResponse<IEnumerable<PassengerModel>>.Fail("An error occurred while retrieving passengers");
            }
        }
    }
}