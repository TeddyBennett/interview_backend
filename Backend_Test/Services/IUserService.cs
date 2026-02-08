using Backend_Test.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponse>> CreateUserAsync(UserCreateRequest request);
        Task<ApiResponse<UserResponse>> GetUserByIdAsync(string id);
        Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync();
        Task<ApiResponse<bool>> UpdateUserAsync(string id, UserCreateRequest request);
        Task<ApiResponse<bool>> DeleteUserAsync(string id);
    }
}
