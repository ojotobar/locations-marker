using LocationMarker.Entities.Models;
using System.Linq.Expressions;

namespace LocationMarker.Data.Interfaces
{
    public interface IStateRepository
    {
        Task AddAsync(List<State> states);
        Task AddAsync(State state);
        Task DeleteAsync(Expression<Func<State, bool>> predicate);
        Task DeleteManyAsync(Expression<Func<State, bool>> predicate, CancellationToken token);
        IQueryable<State> FindAsQueryable(Expression<Func<State, bool>> expression);
        Task<List<State>> FindMany(Expression<Func<State, bool>> expression);
        Task<State?> FindOneAsync(Expression<Func<State, bool>> expression);
        Task<bool> RecordExists(Expression<Func<State, bool>> predicate);
    }
}
