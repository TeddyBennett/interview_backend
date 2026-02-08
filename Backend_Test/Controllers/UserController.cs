using Backend_Test.Attributes;
using Backend_Test.Models;
using Backend_Test.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuthorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService; // Injecting Auth Service

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Authenticates a user and returns a JWT.")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var token = await _authService.UserLoginAsync(model.Username, model.Password);
            if (token == null)
            {
                return Unauthorized(new ApiResponse<object> { Success = false, StatusCode = 401, Message = "Invalid credentials." });
            }
            return Ok(new ApiResponse<object> { Success = true, StatusCode = 200, Data = new { Token = token } });
        }

        [Authorize] // Requires JWT
        [HttpGet("profile")]
        [SwaggerOperation(Summary = "Get Profile of the authenticated user")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _userService.GetUserByIdAsync(userId);
            return Ok(result);
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create User Info")]
        public async Task<IActionResult> CreateUser([FromForm] UserCreateRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            if (result.Success)
                return StatusCode(result.StatusCode, result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get All Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get User By Id")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update User Info")]
        public async Task<IActionResult> UpdateUser(string id, [FromForm] UserCreateRequest request)
        {
            var result = await _userService.UpdateUserAsync(id, request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete User By Id")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
