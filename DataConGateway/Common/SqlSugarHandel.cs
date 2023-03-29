using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConGateway.Common
{
    public class SqlSugarHandel
    {
        private static SqlSugarScope mysqlDb;
        private static SqlSugarScope msSqlDb;
        private static readonly object conlock = new object();
        public static SqlSugarScope GetMySqlDb(string connStr)
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
                            ConnectionString = string.IsNullOrEmpty(connStr) ? "database=OcelotDB; Data Source=127.0.0.1; User Id =root; password=weitianhua;" : connStr, 
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

        public static SqlSugarScope GetMsSqlDb(string connStr)
        {
            if (msSqlDb == null)
            {
                lock (conlock)
                {
                    if (msSqlDb == null)
                    {
                        msSqlDb = new SqlSugarScope(new ConnectionConfig()
                        {
                            DbType = DbType.SqlServer,
                            ConnectionString = string.IsNullOrEmpty(connStr) ? "Data Source=127.0.0.1; Initial Catalog=WiteemDB; User Id=sa; password=weitianhua;" : connStr,
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
}
