using API_DVETS.Services;
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
        private readonly IDocumentService _documentService;
        private readonly ILogger<PassengerService> _logger;
        private readonly MinioService _minioService;

        public PassengerService(
            IPassengerRepository passengerRepository, 
            IDocumentService documentService,
            ILogger<PassengerService> logger, 
            MinioService minioService)
        {
            _passengerRepository = passengerRepository;
            _documentService = documentService;
            _logger = logger;
            _minioService = minioService;
        }

        public async Task<ApiResponse<int>> CreatePassengerAsync(PassengerModel passenger)
        {
            try
            {
                // 1. Create the Document first
                if (passenger.IdfDocTypeId == null || string.IsNullOrEmpty(passenger.IdfDocNumber) || passenger.CountryId == null)
                {
                    return ApiResponse<int>.Fail("Document information is incomplete.");
                }

                var docRequest = new DocumentCreateRequest
                {
                    DocumentTypeId = passenger.IdfDocTypeId.Value,
                    DocumentNumber = passenger.IdfDocNumber,
                    IssuedCountryId = passenger.CountryId.Value,
                    IssueDate = passenger.IssueDate,
                    ExpiryDate = passenger.ExpiryDate
                };

                string docId = await _documentService.CreateDocumentAsync(docRequest);
                passenger.DocId = docId;

                // 2. Handle image upload
                string? faceImageUrl = null;
                if (passenger.FaceImageFile != null)
                {
                    faceImageUrl = await _minioService.UploadFileAsync(passenger.FaceImageFile, "passengers");
                }
                passenger.FaceImageUrl = faceImageUrl;
                passenger.CreatedAt = DateTime.UtcNow;

                // 3. Create Passenger
                int newPassengerId = await _passengerRepository.CreateAsync(passenger);
                _logger.LogInformation("Passenger added successfully with ID: {PassengerId}, DocId: {DocId}", newPassengerId, docId);
                
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

                // If DocId is provided, we use it, otherwise we keep existing
                if (string.IsNullOrEmpty(passenger.DocId))
                {
                    passenger.DocId = existingPassenger.DocId;
                }

                if (passenger.FaceImageFile != null)
                {
                    if (!string.IsNullOrEmpty(existingPassenger.FaceImageUrl))
                    {
                        try { await _minioService.DeleteFileByUrlAsync(existingPassenger.FaceImageUrl); }
                        catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete old passenger image: {Url}", existingPassenger.FaceImageUrl); }
                    }
                    passenger.FaceImageUrl = await _minioService.UploadFileAsync(passenger.FaceImageFile, "passengers");
                }
                else
                {
                    passenger.FaceImageUrl = existingPassenger.FaceImageUrl;
                }

                bool result = await _passengerRepository.UpdateAsync(passenger);
                return ApiResponse<bool>.Ok(result, "Passenger updated successfully");
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
                var existingPassenger = await _passengerRepository.GetByIdAsync(passengerId);
                if (existingPassenger == null)
                    throw new NotFoundException($"Passenger with ID {passengerId} not found");

                if (!string.IsNullOrEmpty(existingPassenger.FaceImageUrl))
                {
                    try { await _minioService.DeleteFileByUrlAsync(existingPassenger.FaceImageUrl); }
                    catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete passenger image: {Url}", existingPassenger.FaceImageUrl); }
                }

                bool result = await _passengerRepository.DeleteAsync(passengerId);
                return ApiResponse<bool>.Ok(result, "Passenger deleted successfully");
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
                var passenger = await _passengerRepository.GetByIdAsync(passengerId);
                if (passenger == null)
                    throw new NotFoundException($"Passenger with ID {passengerId} not found");

                return ApiResponse<PassengerModel>.Ok(passenger, "Passenger retrieved successfully");
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
