using System;
using DataConGateway.Common;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using Newtonsoft.Json;
using Ocelot.Configuration;

namespace DataConGateway.OcelotConfigExtends;

public class RedisFileConfigurationRepository : IFileConfigurationRepository
{
    private readonly OcelotConfigOptions _options;
    public RedisFileConfigurationRepository(OcelotConfigOptions options)
    {
        _options = options;
    }

    public async Task<Response<FileConfiguration>> Get()
    {
        
        var file = new FileConfiguration();
        var reidsKeys = await RedisHelper.KeysAsync("*");
        if (RedisHelper.Exists("ocelot:config1"))
        {
            var result = await RedisHelper.GetAsync<OcelotRedis>("ocelot:config1");
            if (result != null)
            {
                FileGlobalConfiguration glb = BuildGlobalConfig(result);
                file.GlobalConfiguration = glb;
                if (result.Routes?.Count > 0)
                {
                    foreach (var item in result.Routes)
                    {
                        // FileRoute routes = BuildRoute(item);
                        file.Routes.Add(item);
                    }
                }
            }
        }
        else
        {
            throw new Exception("未监测到任何可用的配置信息");
        }

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
    private FileRoute BuildRoute(OcelotRoutes item)
    {
        var routes = new FileRoute();
        routes.Key = Guid.NewGuid().ToString();
        routes.Priority = item.Priority;
        routes.RequestIdKey = item.RequestIdKey;
        if (item.LoadBalancerOptions != null)
            routes.LoadBalancerOptions = item.LoadBalancerOptions;

        if (item.QoSOptions != null)
            routes.QoSOptions = item.QoSOptions;

        routes.ServiceName = item.ServiceName;
        routes.UpstreamHttpMethod = item.UpstreamHttpMethod;
        routes.UpstreamPathTemplate = item.UpstreamPathTemplate;
        routes.DownstreamScheme = item.DownstreamScheme;
        routes.DownstreamPathTemplate = item.DownstreamPathTemplate;
        routes.RouteIsCaseSensitive = false; // 路由是否区分大小写
        routes.DangerousAcceptAnyServerCertificateValidator = false;
        return routes;
    }

    private FileGlobalConfiguration BuildGlobalConfig(OcelotRedis result)
    {
        var glb = new FileGlobalConfiguration();
        glb = _options.GlobalConfiguration;

        //glb.BaseUrl = result.GlobalConfiguration.BaseUrl;
        //glb.ServiceDiscoveryProvider = result.GlobalConfiguration.ServiceDiscoveryProvider;
        //glb.RequestIdKey = result.GlobalConfiguration.RequestIdKey;
        //if (result.GlobalConfiguration.HttpHandlerOptions != null)
        //{
        //    glb.HttpHandlerOptions = result.GlobalConfiguration.HttpHandlerOptions;
        //}

        //if (result.GlobalConfiguration.LoadBalancerOptions != null)
        //{
        //    glb.LoadBalancerOptions = result.GlobalConfiguration.LoadBalancerOptions;
        //}

        //if (result.GlobalConfiguration.QoSOptions != null)
        //{
        //    glb.QoSOptions = result.GlobalConfiguration.QoSOptions;
        //}

        return glb;
    }
    #endregion
}

