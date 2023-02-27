namespace DataConApplication;

public interface IWxUserAppService : IAppServers
{
    Task<string> AppServiceTest();
}
