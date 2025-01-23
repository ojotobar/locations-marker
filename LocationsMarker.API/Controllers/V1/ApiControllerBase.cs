using API.Common.Response.Model.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LocationsMarker.API.Controllers.V1
{
    public class ApiControllerBase : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ProcessError(ApiBaseResponse baseResponse)
        {
            return baseResponse switch
            {
                NotFoundResponse => NotFound((NotFoundResponse)baseResponse),
                BadRequestResponse => BadRequest((BadRequestResponse)baseResponse),
                UnauthorizedResponse => Unauthorized((UnauthorizedResponse)baseResponse),
                ForbiddenResponse => BadRequest((ForbiddenResponse)baseResponse),
                InternalServerErrorResponse => BadRequest((InternalServerErrorResponse)baseResponse),
                _ => throw new NotImplementedException()
            };
        }
    }
}
