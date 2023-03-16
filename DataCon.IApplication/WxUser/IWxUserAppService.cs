using DataConCore.TableEntitys;

namespace DataCon.IApplication;

public interface IWxUserAppService : IAppServers
{
    Task<List<UserEntity>> AppServiceTest();

    Task<object> MyTestAsync();
}
