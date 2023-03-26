namespace DataCon.IApplication;
using System;


public interface IMenuAppService : IAppServers
{
    Task<string> GetMenus(CancellationToken cancellation);
}

