using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.Models;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using System.Linq.Expressions;

namespace LocationMarker.Data.Implementations
{
    public sealed class CountryRepository(MongoDbSettings settings) 
        : Repository<Country>(settings), ICountryRepository
    {
        public async Task AddAsync(Country country) =>
            await CreateAsync(country);

        public async Task AddAsync(List<Country> countries) =>
            await CreateManyAsync(countries);

        public async Task DeleteAsync(Expression<Func<Country, bool>> predicate)
            => await RemoveAsync(predicate);

        public async Task DeleteManyAsync(Expression<Func<Country, bool>> predicate, CancellationToken token)
            => await RemoveManyAsync(predicate, token);

        public async Task<bool> RecordExists(Expression<Func<Country, bool>> predicate)
            => await ExistsAsync(predicate);

        public IQueryable<Country> FindAsQueryable(Expression<Func<Country, bool>> expression) =>
            GetAsQueryable(expression);

        public async Task<Country?> FindOneAsync(Expression<Func<Country, bool>> expression) =>
            await GetAsync(expression);
    }
}
