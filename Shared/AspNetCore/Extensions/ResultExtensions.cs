using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Response;

namespace Shared.AspNetCore.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this ControllerBase controller, Result result)
        {
            if (result.IsSuccess)
                return controller.Ok();

            return controller.HandleFailure(result);
        }

        public static IActionResult ToActionResult<T>(this ControllerBase controller, ResultT<T> result)
        {
            if (result.IsSuccess)
                return controller.Ok(result.Value);

            return controller.HandleFailure(result);
        }

        private static IActionResult HandleFailure(this ControllerBase controller, Result result)
        {
            var error = result.Error.FirstOrDefault() ?? Error.BadRequest;

            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,

                ErrorType.NotFound => StatusCodes.Status404NotFound,

                ErrorType.Conflict => StatusCodes.Status409Conflict,

                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,

                ErrorType.Forbidden => StatusCodes.Status403Forbidden,

                _ => StatusCodes.Status500InternalServerError
            };

            return controller.StatusCode(statusCode, result.Error);
        }
    }
}
