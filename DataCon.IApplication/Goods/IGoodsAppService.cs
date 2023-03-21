using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCon.IApplication
{
    public interface IGoodsAppService : IAppServers
    {
        Task<string> GetGoodsByUser(string user, CancellationToken cancellation);

        Task<string> GetGoods(CancellationToken cancellation);
    }
}
