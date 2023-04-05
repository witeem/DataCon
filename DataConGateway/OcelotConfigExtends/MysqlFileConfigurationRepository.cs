using DataConGateway.Common;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using SqlSugar;
using Newtonsoft.Json;

namespace DataConGateway.OcelotConfigExtends;

/// <summary>
/// 使用mysql 实现配置文件仓储接口
/// </summary>
public class MysqlFileConfigurationRepository : IFileConfigurationRepository
{
    private readonly OcelotConfigOptions _options;
    public MysqlFileConfigurationRepository(OcelotConfigOptions options)
    {
        _options = options;
    }

    public async Task<Response<FileConfiguration>> Get()
    {
        var sqlSugar = SqlSugarHandel.GetMySqlDb(_options.MySqlConnection);
        var file = new FileConfiguration();
        // 查询数据库配置信息
        var result = await sqlSugar.Queryable<OcelotEntity>().Where(m => m.IsDel == 0).FirstAsync();
        if (result != null)
        {
            FileGlobalConfiguration glb = BuildGlobalConfig(result);
            file.GlobalConfiguration = glb;

            var routeList = await sqlSugar.Queryable<OcelotRouteEntity>().Where(m => m.IsDel == 0).ToListAsync();
            if (routeList?.Count > 0)
            {
                foreach (var item in routeList)
                {
                    FileRoute routes = BuildRoute(item);
                    file.Routes.Add(routes);
                }
            }
        }
        else 
        {
            throw new Exception("未监测到任何可用的配置信息");
        }
        // 将获取的结果赋值到 file

        return new OkResponse<FileConfiguration>(file);
    }

    

    /// <summary>
    /// 使用了数据库存储，可以不实现接口直接返回
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<Response> Set(FileConfiguration file)
    {
        await Task.CompletedTask;
        return new OkResponse();
    }

    #region Private

    private FileGlobalConfiguration BuildGlobalConfig(OcelotEntity result)
    {
        var glb = new FileGlobalConfiguration();
        glb.BaseUrl = result.BaseUrl;
        glb.RequestIdKey = result.RequestIdKey;
        if (!string.IsNullOrEmpty(result.HttpHandlerOptions))
            glb.HttpHandlerOptions = JsonConvert.DeserializeObject<FileHttpHandlerOptions>(result.HttpHandlerOptions);

        if (!string.IsNullOrEmpty(result.ServiceDiscoveryProvider))
            glb.ServiceDiscoveryProvider = JsonConvert.DeserializeObject<FileServiceDiscoveryProvider>(result.ServiceDiscoveryProvider);

        if (!string.IsNullOrEmpty(result.LoadBalancerOptions))
            glb.LoadBalancerOptions = JsonConvert.DeserializeObject<FileLoadBalancerOptions>(result.LoadBalancerOptions);

        if (!string.IsNullOrEmpty(result.QoSOptions))
            glb.QoSOptions = JsonConvert.DeserializeObject<FileQoSOptions>(result.QoSOptions);

        return glb;
    }

    private FileRoute BuildRoute(OcelotRouteEntity item)
    {
        var routes = new FileRoute();
        routes.Key = Guid.NewGuid().ToString();
        routes.Priority = item.Priority;
        routes.RequestIdKey = item.RequestIdKey;
        if (!string.IsNullOrEmpty(item.LoadBalancerOptions))
            routes.LoadBalancerOptions = JsonConvert.DeserializeObject<FileLoadBalancerOptions>(item.LoadBalancerOptions);

        if (!string.IsNullOrEmpty(item.QoSOptions))
            routes.QoSOptions = JsonConvert.DeserializeObject<FileQoSOptions>(item.QoSOptions);

        if (!string.IsNullOrEmpty(item.UpstreamHttpMethod))
            routes.UpstreamHttpMethod = JsonConvert.DeserializeObject<List<string>>(item.UpstreamHttpMethod);

        routes.ServiceName = item.ServiceName;
        routes.UpstreamPathTemplate = item.UpstreamPathTemplate;
        routes.DownstreamScheme = item.DownstreamScheme;
        routes.DownstreamPathTemplate = item.DownstreamPathTemplate;
        routes.RouteIsCaseSensitive = false; // 路由是否区分大小写
        routes.DangerousAcceptAnyServerCertificateValidator = false;
        return routes;
    }
    #endregion
}
