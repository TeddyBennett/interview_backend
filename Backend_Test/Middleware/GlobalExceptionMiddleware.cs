using Backend_Test.Exceptions;
using Backend_Test.Models;
using System.Text.Json;
using ValidationException = Backend_Test.Exceptions.ValidationException;

namespace Backend_Test.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>
            {
                Success = false,
                Data = null,
                Errors = null
            };

            switch (exception)
            {
                case ValidationException ve:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = ve.Message;
                    response.Errors = ve.Errors;
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.Message = exception.Message;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Message = exception.Message; // ✅ โชว์ message ที่โยนจาก repository/service
                    break;
            }

            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }
    }
}
