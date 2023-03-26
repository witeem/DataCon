namespace DataCon.Application;
using System;
using System.Threading.Tasks;
using DataCon.Application;
using DataCon.IApplication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class MenuAppService : BaseApplication, IMenuAppService
{
    private readonly ILogger<MenuAppService> _logger;

    public MenuAppService(ILogger<MenuAppService> logger
        )
    {
        _logger = logger;
    }

    public async Task<string> GetMenus(CancellationToken cancellation)
    {
        List<string> menus = new List<string> { "WxUser", "Goods", "Menus" };
        await Task.CompletedTask;
        return JsonConvert.SerializeObject(menus);
    }
}

