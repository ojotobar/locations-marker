using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.Models;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using System.Linq.Expressions;

namespace LocationMarker.Data.Implementations
{
    public class ApiKeyRepository(MongoDbSettings settings) : Repository<ApiKeys>(settings), IApiKeyRepository
    {
        public async Task AddAsync(ApiKeys apiKeys) =>
            await CreateAsync(apiKeys);

        public async Task EditAsync(Expression<Func<ApiKeys, bool>> expression, ApiKeys apiKeys) =>
            await UpdateAsync(expression, apiKeys);

        public async Task<bool> KeyExistsAsync(Expression<Func<ApiKeys, bool>> expression) =>
            await ExistsAsync(expression);

        public async Task<ApiKeys?> FindAsync(Expression<Func<ApiKeys, bool>> expression) =>
            await GetAsync(expression);
    }
}
