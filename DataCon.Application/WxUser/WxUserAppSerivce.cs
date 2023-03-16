using DataCon.Application;
using DataCon.IApplication;
using DataCon.IRepositories.WxUser;
using DataConCore.TableEntitys;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace DataConApplication;

public class WxUserAppSerivce : BaseApplication, IWxUserAppService
{
    private readonly ILogger<WxUserAppSerivce> _logger;
    private readonly IWxUserRepositories _wxUserRepositories;
    public WxUserAppSerivce(ILogger<WxUserAppSerivce> logger,
        IWxUserRepositories wxUserRepositories
        )
    {
        _logger = logger;
        _wxUserRepositories = wxUserRepositories;
    }

    public async Task<List<UserEntity>> AppServiceTest()
    {
        _logger.LogInformation("Application服务");
        var query = _wxUserRepositories.QueryList(m => m.IsDel == 0);
        var list = await query.ToListAsync();
        return list;
        
        /*
        await Task.CompletedTask;
        return new List<UserEntity>();*/
    }

    public async Task<object> MyTestAsync()
    {
        _logger.LogInformation(nameof(MyTestAsync));
        var list = new { Code = 1, Name = nameof(MyTestAsync) };
        await Task.CompletedTask;
        return list;
    }
}
