using Backend_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend_Test.Validators
{
    public class ApiResponseFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // ไม่ต้องทำอะไร
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                return; // ให้ Middleware จัดการ

            if (context.Result is ObjectResult objectResult)
            {
                var statusCode = objectResult.StatusCode ?? 200;

                if (objectResult.Value is IApiResponse)
                    return;

                context.Result = new ObjectResult(ApiResponse<object>.Ok(objectResult.Value))
                {
                    StatusCode = statusCode
                };
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(ApiResponse<object>.Ok(null, "Success"))
                {
                    StatusCode = 204
                };
            }
        }

    }
}
