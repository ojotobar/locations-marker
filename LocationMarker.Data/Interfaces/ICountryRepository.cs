using LocationMarker.Entities.Models;
using System.Linq.Expressions;

namespace LocationMarker.Data.Interfaces
{
    public interface ICountryRepository
    {
        Task AddAsync(Country country);
        Task AddAsync(List<Country> countries);
        Task DeleteAsync(Expression<Func<Country, bool>> predicate);
        Task DeleteManyAsync(Expression<Func<Country, bool>> predicate, CancellationToken token);
        IQueryable<Country> FindAsQueryable(Expression<Func<Country, bool>> expression);
        Task<Country?> FindOneAsync(Expression<Func<Country, bool>> expression);
        Task<bool> RecordExists(Expression<Func<Country, bool>> predicate);
    }
}
