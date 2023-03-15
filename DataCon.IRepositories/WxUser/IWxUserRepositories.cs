using DataConCore.TableEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCon.IRepositories.WxUser
{
    public interface IWxUserRepositories : IAppRepositories, IBaseRepositories<UserEntity>
    {
    }
}
