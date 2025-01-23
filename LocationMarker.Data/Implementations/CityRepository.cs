using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.Models;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using System.Linq.Expressions;

namespace LocationMarker.Data.Implementations
{
    public sealed class CityRepository(MongoDbSettings settings) : Repository<City>(settings), 
        ICityRepository
    {
        public async Task AddAsync(City city) =>
           await CreateAsync(city);

        public async Task AddAsync(List<City> cities) =>
            await CreateManyAsync(cities);

        public async Task DeleteAsync(Expression<Func<City, bool>> predicate)
            => await RemoveAsync(predicate);

        public async Task DeleteManyAsync(Expression<Func<City, bool>> predicate, CancellationToken token)
            => await RemoveManyAsync(predicate, token);

        public async Task<bool> RecordExists(Expression<Func<City, bool>> predicate) =>
            await ExistsAsync(predicate);

        public IQueryable<City> FindAsQueryable(Expression<Func<City, bool>> expression) =>
            GetAsQueryable(expression);

        public async Task<City?> FindOneAsync(Expression<Func<City, bool>> expression) =>
            await GetAsync(expression);
    }
}
