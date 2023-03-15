using DataCon.IRepositories;
using DataCon.IRepositories.WxUser;
using DataConCore.TableEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCon.Repositories.WxUser
{
    public class WxUserRepositories : BaseRepositories<UserEntity>, IWxUserRepositories
    {
    }
}
