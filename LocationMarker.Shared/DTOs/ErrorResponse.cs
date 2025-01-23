using API.Common.Response.Model.Responses;
using System.Net;

namespace LocationMarker.Shared.DTOs
{
    public class ErrorResponse
    {
        public static ApiBaseResponse Map(HttpStatusCode code, string message = "")
        {
            return code switch
            {
                HttpStatusCode.BadRequest => new BadRequestResponse(message),
                HttpStatusCode.Unauthorized => new UnauthorizedResponse(message),
                HttpStatusCode.Forbidden => new ForbiddenResponse(message),
                HttpStatusCode.NotFound => new NotFoundResponse(message),
                _ => new InternalServerErrorResponse(message)
            };
        }
    }
}
