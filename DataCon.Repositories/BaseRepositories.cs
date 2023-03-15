using DataCon.IRepositories;
using DataConCore;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataCon.Repositories
{
    public abstract class BaseRepositories<TEntity> : IBaseRepositories<TEntity>
    {
        private readonly SqlSugarScope sqlDb;
        public BaseRepositories()
        {
            sqlDb = SqlSugarHandel.GetMySqlDb();
        }

        public ISugarQueryable<TEntity> QueryList(Expression<Func<TEntity, bool>> where)
        {
            var query = sqlDb.Queryable<TEntity>().Where(where);
            return query;
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where)
        {
            var list = await QueryList(where).ToListAsync();
            return list;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            var obj = sqlDb.Queryable<TEntity>().FirstAsync(where);
            return await obj;
        }

        public async Task<long> AddAsync(object newData)
        {
            //插入返回自增列 (实体除ORACLE外实体要配置自增，Oracle需要配置序列)
            var objId = sqlDb.Insertable(newData).ExecuteReturnBigIdentityAsync();
            return await objId;
        }

        public async Task<long> UpdateAsync(object newData)
        {
            //插入返回自增列 (实体除ORACLE外实体要配置自增，Oracle需要配置序列)
            var objId = sqlDb.Updateable(newData).ExecuteCommandAsync();
            return await objId;
        }
    }
}
