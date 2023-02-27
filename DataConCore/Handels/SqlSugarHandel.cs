using SqlSugar;
using System;


namespace DataConCore;

public class SqlSugarHandel
{
    private static SqlSugarScope mysqlDb;
    private static SqlSugarScope msSqlDb;
    private static readonly object conlock = new object();
    public static SqlSugarScope GetMySqlDb()
    {
        if (mysqlDb == null)
        {
            lock (conlock)
            {
                if (mysqlDb == null)
                {
                    mysqlDb = new SqlSugarScope(new ConnectionConfig()
                    {
                        DbType = DbType.MySql,
                        ConnectionString = "database=WiteemDB; Data Source=127.0.0.1; User Id =root; password=weitianhua;",
                        InitKeyType = InitKeyType.Attribute,
                        IsAutoCloseConnection = true,
                        AopEvents = new AopEvents()
                    },
                    db =>
                    {
                        db.Aop.OnLogExecuting = (sql, p) =>
                        {
                            Console.WriteLine(sql);
                        };
                    });
                }
            }
        }

        return mysqlDb;
    }

    public static SqlSugarScope GetMsSqlDb()
    {
        if (msSqlDb == null)
        {
            lock(conlock)
            {
                if (msSqlDb == null)
                {
                    msSqlDb = new SqlSugarScope(new ConnectionConfig()
                    {
                        DbType = DbType.SqlServer,
                        ConnectionString = "Data Source=127.0.0.1; Initial Catalog=WiteemDB; User Id=sa; password=weitianhua;",
                        InitKeyType = InitKeyType.Attribute,
                        IsAutoCloseConnection = true,
                        AopEvents = new AopEvents()
                    });
                }
            }

        }
        return msSqlDb;
    }
}
