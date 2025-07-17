using Microsoft.AspNetCore.Mvc;

namespace RentACarProject.Application.Common
{
    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult<T>(this ControllerBase controller, ServiceResponse<T> response)
        {
            if (response.Success)
                return controller.Ok(response);

            return response.Code switch
            {
                "400" => controller.BadRequest(new { message = response.Message }),
                "401" => controller.Unauthorized(new { message = response.Message }),
                "403" => controller.Forbid(),
                "404" => controller.NotFound(new { message = response.Message }),
                "500" => controller.StatusCode(500, new { message = response.Message }),
                _ => controller.BadRequest(new { message = response.Message })
            };
        }
    }
}
