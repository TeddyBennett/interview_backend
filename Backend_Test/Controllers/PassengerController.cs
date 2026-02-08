using Backend_Test.Attributes;
using Backend_Test.Models;
using Backend_Test.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly PassengerService _passengerService;

        public PassengerController(PassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [Authorize]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add Passenger Info")]
        [HideFields("passenger_id", "face_image", "face_image_name")]
        public async Task<IActionResult> AddPassenger([FromForm] PassengerModel passenger)
        {
            var result = await _passengerService.AddPassenger(passenger);

            if (result.Success)
                return StatusCode(result.StatusCode, result);

            return BadRequest(result);
        }


        [Authorize]
        [HttpPost("update")]
        [SwaggerOperation(Summary = "Update Passenger Info")]
        [HideFields("passenger_id", "face_image", "face_image_name")]
        public async Task<IActionResult> UpdatePassenger([FromQuery] int passengerId, [FromForm] PassengerModel passenger)
        {
            var result = await _passengerService.UpdatePassenger(passenger, passengerId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize]
        [HttpPost("delete/{passengerId}")]
        [SwaggerOperation(Summary = "Delete Passenger By Passenger Id")]
        public async Task<IActionResult> DeletePassenger(int passengerId)
        {
            var result = await _passengerService.DeletePassenger(passengerId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("{passengerId}")]
        [SwaggerOperation(Summary = "Get Passenger By Passenger Id")]
        public async Task<IActionResult> GetPassengerById(int passengerId)
        {
            var result = await _passengerService.GetPassengerById(passengerId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
    }
}
