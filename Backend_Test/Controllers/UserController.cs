using Backend_Test.Attributes;
using Backend_Test.Models;
using Backend_Test.Services;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
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

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get All Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get User By Id")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update User Info")]
        public async Task<IActionResult> UpdateUser(string id, [FromForm] UserCreateRequest request)
        {
            var result = await _userService.UpdateUserAsync(id, request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

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
