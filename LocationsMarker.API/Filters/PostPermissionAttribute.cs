using Microsoft.AspNetCore.Mvc;

namespace LocationsMarker.API.Filters
{
    public class PostPermissionAttribute : TypeFilterAttribute
    {
        public PostPermissionAttribute() : base(typeof(PermissionFilter))
        {
        }
    }
}
