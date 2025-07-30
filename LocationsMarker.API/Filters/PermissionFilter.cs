using API.Common.Response.Model.Responses;
using CSharpTypes.Extensions.List;
using LocationMarker.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace LocationsMarker.API.Filters
{
    public class PermissionFilter : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<PermissionFilter> _logger;

        public PermissionFilter(ILogger<PermissionFilter> logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }
        
        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var apiKey = context.HttpContext.Request.Headers["X-Marker-API-Key"];
            if (apiKey.IsNotNullOrEmpty())
            {
                await ValidateApiKey(context, apiKey);
            }
        }

        private async Task ValidateApiKey(ActionExecutingContext context, StringValues apiKey)
        {
            var savedApiKey = await _repository.ApiKey.FindAsync(k => !k.IsDeprecated);
            if (savedApiKey != null)
            {
                if (savedApiKey.ApiKey.Equals(apiKey))
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

    }
}
