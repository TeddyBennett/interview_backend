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
    [ApiKeyAuthorize] // Level 1: App Validation
    [Authorize]       // Level 2: User Validation (Requires JWT)
    public class PassengerController : ControllerBase
    {
        private readonly PassengerService _passengerService;

        public PassengerController(PassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create Passenger Info")]
        public async Task<IActionResult> CreatePassenger([FromForm] PassengerModel passenger)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _passengerService.CreatePassengerAsync(passenger, userId);

            if (result.Success)
                return StatusCode(result.StatusCode, result);

            return BadRequest(result);
        }

        [HttpPut("{passengerId}")]
        [SwaggerOperation(Summary = "Update Passenger Info")]
        public async Task<IActionResult> UpdatePassenger(int passengerId, [FromForm] PassengerModel passenger)
        {
            passenger.PassengerId = passengerId; // Set the ID from route to model
            var result = await _passengerService.UpdatePassengerAsync(passenger);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{passengerId}")]
        [SwaggerOperation(Summary = "Delete Passenger By ID")]
        public async Task<IActionResult> DeletePassenger(int passengerId)
        {
            var result = await _passengerService.DeletePassengerAsync(passengerId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("{passengerId}")]
        [SwaggerOperation(Summary = "Get Passenger By ID")]
        public async Task<IActionResult> GetPassengerById(int passengerId)
        {
            var result = await _passengerService.GetPassengerByIdAsync(passengerId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get All Passengers")]
        public async Task<IActionResult> GetAllPassengers()
        {
            var result = await _passengerService.GetAllPassengersAsync();

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}