using DataConCore.TableEntitys;
using Newtonsoft.Json;
namespace DataConApplication;

public class WxUserAppSerivce : IWxUserAppService
{
    public WxUserAppSerivce()
    {
    }

    public async Task<string> AppServiceTest()
    {
        Console.WriteLine("测试注入业务层");
        UserEntity entity = new UserEntity();
        var query = entity.QueryList(m=> m.WxName == "WxUser");
        var list = await query.ToListAsync();
        return JsonConvert.SerializeObject(list);
    }
}
