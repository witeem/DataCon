using SqlSugar;
using System.Linq.Expressions;
namespace DataConCore.TableEntitys;

public abstract class BaseEntity<T> : IBaseEntity<T>
{
    private readonly SqlSugarScope sqlDb;
    public BaseEntity()
    {
        sqlDb = SqlSugarHandel.GetMySqlDb();
    }

    public ISugarQueryable<T> QueryList(Expression<Func<T, bool>> where)
    {
        var query = sqlDb.Queryable<T>().Where(where);
        return query;
    }

    public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> where)
    {
        var list = await QueryList(where).ToListAsync();
        return list;
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> where)
    {
        var obj = sqlDb.Queryable<T>().FirstAsync(where);
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
