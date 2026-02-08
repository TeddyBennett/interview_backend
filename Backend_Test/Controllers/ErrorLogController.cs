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
    public class ErrorLogController : ControllerBase
    {
        private readonly ErrorLogService _service;

        public ErrorLogController(ErrorLogService service)
        {
            _service = service;
        }

        [HttpPost("SaveErrorLog")]
        [SwaggerOperation(Summary = "Save Error Log Data")]
        [HideFields("id")]
        public async Task<IActionResult> SaveErrorLog(
    [FromBody] ErrorLogModel data)
        {
            var success = await _service.SaveLogData(data);
            return Ok(new { success, message = "Save error logs successfully" });
        }
    }
}
