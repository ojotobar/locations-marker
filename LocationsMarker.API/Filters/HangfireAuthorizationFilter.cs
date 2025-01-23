using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;

namespace LocationsMarker.API.Filters
{
    public class HangfireAuthorizationFilter(IConfiguration configuration) : IDashboardAuthorizationFilter
    {
        private readonly IConfiguration configuration = configuration;

        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var header = httpContext.Request.Headers.Authorization;

            if (string.IsNullOrEmpty(header))
            {
                SetResponse(httpContext);
                return false;
            }

            var authvalues = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

            if (!"Basic".Equals(authvalues.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                SetResponse(httpContext);
                return false;
            }

            var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authvalues.Parameter!));
            var parts = parameter.Split(':');

            var username = parts[0];
            var password = parts[1];


            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                SetResponse(httpContext);
                return false;
            }

            var pass = configuration["Hangfire:Password"];
            var userName = configuration["Hangfire:UserName"];
            if (password == pass && username == userName)
            {
                return true;
            }

            SetResponse(httpContext);
            return false;
        }

        private static void SetResponse(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            httpContext.Response.WriteAsync("Authentication is required");
        }
    }
}
