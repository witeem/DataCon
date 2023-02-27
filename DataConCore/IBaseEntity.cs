using System.Linq.Expressions;
using SqlSugar;

namespace DataConCore;

public interface IBaseEntity<T>
{
    ISugarQueryable<T> QueryList(Expression<Func<T, bool>> where);

    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> where);

    Task<T> GetAsync(Expression<Func<T, bool>> where);

    Task<long> AddAsync(object value);

    Task<long> UpdateAsync(object value);
}
