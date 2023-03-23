using DataConCore.TableEntitys;

namespace DataCon.IApplication;

public interface IWxUserAppService : IAppServers
{
    Task<List<UserEntity>> AppServiceTest();

    Task<object> MyTestAsync();

    string SendMqAsync();

    string ExpendMqAsync();

    string CloseExpendMqAsync();
}
