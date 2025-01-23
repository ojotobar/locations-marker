using LocationMarker.Entities.Models;
using System.Linq.Expressions;

namespace LocationMarker.Data.Interfaces
{
    public interface IApiKeyRepository
    {
        Task AddAsync(ApiKeys apiKeys);
        Task EditAsync(Expression<Func<ApiKeys, bool>> expression, ApiKeys apiKeys);
        Task<ApiKeys?> FindAsync(Expression<Func<ApiKeys, bool>> expression);
        Task<bool> KeyExistsAsync(Expression<Func<ApiKeys, bool>> expression);
    }
}
