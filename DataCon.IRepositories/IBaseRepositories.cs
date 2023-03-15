using DataConCore.TableEntitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataCon.IRepositories
{
    public interface IBaseRepositories<TEntity>: IAppRepositories
    {
        ISugarQueryable<TEntity> QueryList(Expression<Func<TEntity, bool>> where);

        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where);

        Task<long> AddAsync(object value);

        Task<long> UpdateAsync(object value);
    }
}
