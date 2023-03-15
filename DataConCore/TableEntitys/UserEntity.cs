using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;

namespace DataConCore.TableEntitys
{
    public class UserEntity : BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        public string WxName { get; set; }

        public string WxPwd { get; set; }

        public string Avator { get; set; }

        public int Age { get; set; }

        public DateTime CreateTime { get; set; }

        public int IsDel { get; set; }
    }
}