using DataConCore.TableEntitys;

namespace DataConApplication;

public interface IWxUserAppService : IAppServers
{
    Task<List<UserEntity>> AppServiceTest();

    Task<object> MyTestAsync();
}
