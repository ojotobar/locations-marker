using CSharpTypes.Extensions.String;
using LocationMarker.Entities.Models;

namespace LocationMarker.Shared.Extensions
{
    public static class RepositoryExtensions
    {
        public static IQueryable<Country> Search(this IQueryable<Country> query, string searchString)
        {
            if (searchString.IsNullOrEmpty())
            {
                return query;
            }

            return query.Where(c => c.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
