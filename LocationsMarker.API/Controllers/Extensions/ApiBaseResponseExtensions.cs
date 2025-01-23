using API.Common.Response.Model.Responses;

namespace LocationsMarker.API.Controllers.Extensions
{
    public static class ApiBaseResponseExtensions
    {
        public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse)
        {
            var ok = (OkResponse<TResultType>)apiBaseResponse;
            return ok.Result;
        }
    }
}
