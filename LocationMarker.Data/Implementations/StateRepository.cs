using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.Models;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using System.Linq.Expressions;

namespace LocationMarker.Data.Implementations
{
    public class StateRepository(MongoDbSettings settings) : Repository<State>(settings),
        IStateRepository
    {
        public async Task AddAsync(State state) =>
            await CreateAsync(state);

        public async Task AddAsync(List<State> states) =>
            await CreateManyAsync(states);

        public async Task DeleteAsync(Expression<Func<State, bool>> predicate)
            => await RemoveAsync(predicate);

        public async Task DeleteManyAsync(Expression<Func<State, bool>> predicate, CancellationToken token)
            => await RemoveManyAsync(predicate, token);

        public async Task<bool> RecordExists(Expression<Func<State, bool>> predicate) =>
            await ExistsAsync(predicate);

        public IQueryable<State> FindAsQueryable(Expression<Func<State, bool>> expression) =>
            GetAsQueryable(expression);

        public async Task<State?> FindOneAsync(Expression<Func<State, bool>> expression) =>
            await GetAsync(expression);

        public async Task<List<State>> FindMany(Expression<Func<State, bool>> expression) =>
            await GetManyAsync(expression);
    }
}
