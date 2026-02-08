using Backend_Test.Models;
using Backend_Test.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend_Test.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var token = await _authService.LoginAsync(model.Username, model.Password);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            return Ok(new { token });
        }

        [HttpPost("register-initial-admin")]
        public async Task<IActionResult> RegisterInitial([FromBody] LoginRequestModel model)
        {
            try
            {
                var admin = await _authService.RegisterAdminAsync(model.Username, model.Password);
                return Ok(admin);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("apikeys")]
        public async Task<IActionResult> CreateApiKey([FromBody] ApiKeyCreationRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int adminId))
            {
                return Unauthorized();
            }

            var rawKey = await _apiKeyService.CreateApiKeyAsync(request.ClientName, adminId, request.ExpiresAt);

            return Ok(new
            {
                ClientName = request.ClientName,
                ApiKey = rawKey,
                Warning = "Copy this key now. You will not be able to see it again."
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("apikeys")]
        public async Task<IActionResult> GetAllApiKeys()
        {
            var keys = await _apiKeyService.GetAllApiKeysAsync();
            return Ok(keys);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("apikeys/{id}")]
        public async Task<IActionResult> GetApiKey(int id)
        {
            var key = await _apiKeyService.GetApiKeyByIdAsync(id);
            if (key == null) return NotFound();
            return Ok(key);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("apikeys/{id}/toggle")]
        public async Task<IActionResult> ToggleApiKey(int id, [FromQuery] bool isEnabled)
        {
            var success = await _apiKeyService.ToggleApiKeyStatusAsync(id, isEnabled);
            if (!success) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("apikeys/{id}")]
        public async Task<IActionResult> DeleteApiKey(int id)
        {
            var success = await _apiKeyService.DeleteApiKeyAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
