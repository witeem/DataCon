using DataCon.IApplication;
using DataConApplication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCon.Application;

public class GoodsAppService : BaseApplication, IGoodsAppService
{
    private readonly ILogger<GoodsAppService> _logger;
    private readonly Lazy<IWxUserAppService> _wxUserApp;

    public GoodsAppService(ILogger<GoodsAppService> logger,
        Lazy<IWxUserAppService> wxUserApp
        )
    {
        _logger = logger;
        _wxUserApp = wxUserApp;
    }

    public async Task<string> GetGoodsByUser(string user, CancellationToken cancellation)
    {
        await _wxUserApp.Value.MyTestAsync();
        return "商品接口";
    }

    public async Task<string> GetGoods(CancellationToken cancellation)
    {
        await Task.CompletedTask;
        Console.WriteLine("获取商品信息");
        return "商品信息";
    }
}
