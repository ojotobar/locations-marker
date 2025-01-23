using LocationMarker.Entities.Models;
using System.Linq.Expressions;

namespace LocationMarker.Data.Interfaces
{
    public interface ICityRepository
    {
        Task AddAsync(List<City> cities);
        Task AddAsync(City city);
        Task DeleteAsync(Expression<Func<City, bool>> predicate);
        Task DeleteManyAsync(Expression<Func<City, bool>> predicate, CancellationToken token);
        IQueryable<City> FindAsQueryable(Expression<Func<City, bool>> expression);
        Task<City?> FindOneAsync(Expression<Func<City, bool>> expression);
        Task<bool> RecordExists(Expression<Func<City, bool>> predicate);
    }
}
