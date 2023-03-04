using DataConCore.TableEntitys;
using Newtonsoft.Json;
namespace DataConApplication;

public class WxUserAppSerivce : IWxUserAppService
{
    public WxUserAppSerivce()
    {
    }

    public async Task<List<UserEntity>> AppServiceTest()
    {
        Console.WriteLine("测试注入业务层");
        UserEntity entity = new UserEntity();
        var query = entity.QueryList(m => m.IsDel == 0);
        var list = await query.ToListAsync();
        return list;
    }
}
