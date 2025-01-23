using API.Common.Response.Model.Responses;
using CSharpTypes.Extensions.List;
using LocationMarker.Data.Interfaces;
using LocationsMarker.API.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace LocationsMarker.API.Filters
{
    public class PermissionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var apiKey = context.HttpContext.Request.Headers["X-Marker-API-Key"];
            if (apiKey.IsNotNullOrEmpty())
            {
                ValidateApiKey(context, apiKey);
            }
            else
            {
                FilterHelper(context);
                return;
            }
            base.OnActionExecuting(context);
        }

        private static void FilterHelper(ActionExecutingContext context)
        {
            var data = new UnauthorizedResponse("You don't have permission to perform this action");
            var result = new JsonResult(data)
            {
                StatusCode = data.StatucCode
            };
            context.HttpContext.Response.StatusCode = data.StatucCode;
            context.Result = result;
            return;
        }

        private static void ValidateApiKey(ActionExecutingContext context, StringValues apiKey)
        {
            var savedApiKey = AppConstants.ApiKey;
            if (savedApiKey != null)
            {
                if (savedApiKey.Equals(apiKey))
                {
                    return;
                }
                else
                {
                    FilterHelper(context);
                    return;
                }
            }
            else
            {
                FilterHelper(context);
                return;
            }
        }
    }
}
