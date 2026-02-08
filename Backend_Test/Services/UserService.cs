using API_DVETS.Services;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using NUlid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly MinioService _minioService;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, MinioService minioService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _minioService = minioService;
        }

        public async Task<ApiResponse<UserResponse>> CreateUserAsync(UserCreateRequest request)
        {
            try
            {
                var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
                if (existingUser != null)
                {
                    return ApiResponse<UserResponse>.Fail("Username already exists");
                }

                string? imageUrl = null;
                if (request.ProfileImage != null)
                {
                    imageUrl = await _minioService.UploadFileAsync(request.ProfileImage, "users");
                }

                var user = new User
                {
                    Id = Ulid.NewUlid().ToString(),
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    FullName = request.FullName,
                    ProfileImageUrl = imageUrl,
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.CreateAsync(user);

                var response = MapToResponse(user);
                return ApiResponse<UserResponse>.Created(response, "User created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return ApiResponse<UserResponse>.Fail("An error occurred while creating the user");
            }
        }

        public async Task<ApiResponse<UserResponse>> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return ApiResponse<UserResponse>.Fail("User not found");
                }

                return ApiResponse<UserResponse>.Ok(MapToResponse(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id {Id}", id);
                return ApiResponse<UserResponse>.Fail("An error occurred while retrieving the user");
            }
        }

        public async Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var response = users.Select(MapToResponse);
                return ApiResponse<IEnumerable<UserResponse>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return ApiResponse<IEnumerable<UserResponse>>.Fail("An error occurred while retrieving users");
            }
        }

        public async Task<ApiResponse<bool>> UpdateUserAsync(string id, UserCreateRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                if (request.ProfileImage != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                    {
                        try { await _minioService.DeleteFileByUrlAsync(user.ProfileImageUrl); }
                        catch { /* Log and ignore if deletion fails */ }
                    }
                    user.ProfileImageUrl = await _minioService.UploadFileAsync(request.ProfileImage, "users");
                }

                user.Username = request.Username;
                user.FullName = request.FullName;

                var result = await _userRepository.UpdateAsync(user);
                return ApiResponse<bool>.Ok(result, "User updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {Id}", id);
                return ApiResponse<bool>.Fail("An error occurred while updating the user");
            }
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return ApiResponse<bool>.Fail("User not found");
                }

                // Delete image from MinIO
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    try { await _minioService.DeleteFileByUrlAsync(user.ProfileImageUrl); }
                    catch { /* Log and ignore */ }
                }

                var result = await _userRepository.DeleteAsync(id);
                return ApiResponse<bool>.Ok(true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {Id}", id);
                return ApiResponse<bool>.Fail("An error occurred while deleting the user");
            }
        }

        private UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                ProfileImageUrl = user.ProfileImageUrl,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
