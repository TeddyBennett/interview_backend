using Backend_Test.Models;
using Backend_Test.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend_Test.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IApiKeyService _apiKeyService;

        public AdminController(IAuthService authService, IApiKeyService apiKeyService)
        {
            _authService = authService;
            _apiKeyService = apiKeyService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Authenticates an administrator and returns a JWT.")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var token = await _authService.LoginAsync(model.Username, model.Password);
            if (token == null)
            {
                return Unauthorized(new ApiResponse<object> { Success = false, StatusCode = 401, Message = "Invalid credentials." });
            }
            return Ok(new ApiResponse<object> { Success = true, StatusCode = 200, Data = new { Token = token } });
        }

        [Authorize(Roles = "Admin")] // Ensure only authorized admins can access
        [HttpPost("apikeys")]
        [SwaggerOperation(Summary = "Creates a new API key for a client. Returns the raw key once.")]
        public async Task<IActionResult> CreateApiKey([FromBody] ApiKeyCreationRequest request)
        {
            var createdById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rawApiKey = await _apiKeyService.CreateApiKeyAsync(request.ClientName, createdById, request.ExpiresAt);
            return Ok(new ApiResponse<object> { Success = true, StatusCode = 200, Data = new { ApiKey = rawApiKey }, Message = "API Key created successfully. Store this key securely, it will not be shown again." });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("apikeys")]
        [SwaggerOperation(Summary = "Retrieves a list of all API keys (excluding the hashed key value).")]
        public async Task<IActionResult> GetAllApiKeys()
        {
            var apiKeys = await _apiKeyService.GetAllApiKeysAsync();
            return Ok(new ApiResponse<IEnumerable<ApiKey>> { Success = true, StatusCode = 200, Data = apiKeys });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("apikeys/{id}")]
        [SwaggerOperation(Summary = "Retrieves details for a single API key.")]
        public async Task<IActionResult> GetApiKeyById(int id)
        {
            var apiKey = await _apiKeyService.GetApiKeyByIdAsync(id);
            if (apiKey == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, StatusCode = 404, Message = "API Key not found." });
            }
            return Ok(new ApiResponse<ApiKey> { Success = true, StatusCode = 200, Data = apiKey });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("apikeys/{id}/disable")]
        [SwaggerOperation(Summary = "Disables an API key, revoking its access.")]
        public async Task<IActionResult> DisableApiKey(int id)
        {
            var success = await _apiKeyService.ToggleApiKeyStatusAsync(id, false);
            if (!success)
            {
                return NotFound(new ApiResponse<object> { Success = false, StatusCode = 404, Message = "API Key not found or unable to update status." });
            }
            return Ok(new ApiResponse<object> { Success = true, StatusCode = 200, Message = "API Key disabled successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("apikeys/{id}/enable")]
        [SwaggerOperation(Summary = "Re-enables a disabled API key.")]
        public async Task<IActionResult> EnableApiKey(int id)
        {
            var success = await _apiKeyService.ToggleApiKeyStatusAsync(id, true);
            if (!success)
            {
                return NotFound(new ApiResponse<object> { Success = false, StatusCode = 404, Message = "API Key not found or unable to update status." });
            }
            return Ok(new ApiResponse<object> { Success = true, StatusCode = 200, Message = "API Key enabled successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("apikeys/{id}")]
        [SwaggerOperation(Summary = "Permanently deletes an API key.")]
        public async Task<IActionResult> DeleteApiKey(int id)
        {
            var success = await _apiKeyService.DeleteApiKeyAsync(id);
            if (!success)
            {
                return NotFound(new ApiResponse<object> { Success = false, StatusCode = 404, Message = "API Key not found or unable to delete." });
            }
            return Ok(new ApiResponse<object> { Success = true, StatusCode = 200, Message = "API Key deleted successfully." });
        }
    }
}