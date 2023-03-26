using DataCon.IApplication;
using DataCon.IRepositories;
using DataCon.Repositories;
using DataConCore;
using DataConCore.Handels;
using DataConCore.Handels.Consul;
using DataConCore.Handels.RabbitMQ;

namespace DataConMenu;

public static class ServicesProvider
{
    /// <summary>
    /// Application服务注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    public static void AddStartupConfigServices(this IServiceCollection services, List<Type> types)
    {
        services.Scan(scan =>
        {
            scan.FromAssembliesOf(types.ToArray())
            .AddClasses(classes => classes.AssignableTo<IAppServers>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
            .AddClasses(classes => classes.AssignableTo<IAppCore>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime();
        });
    }

    /// <summary>
    /// 仓储服务注入
    /// </summary>
    /// <param name="services"></param>
    public static void AddRepositories(this IServiceCollection services)
    {
        List<Type> types = new List<Type> { typeof(BaseRepositories<>) };
        services.Scan(scan =>
        {
            scan.FromAssembliesOf(types.ToArray())
            .AddClasses(classes => classes.AssignableTo<IAppRepositories>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime();
        });
    }

    /// <summary>
    /// 懒加载服务容器注入
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static void AddLazyResolution(this IServiceCollection services)
    {
        services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
    }

    /// <summary>
    /// RabbitMq 构造器注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void MqProducerRegist(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("RabbitMqOptions");
        services.Configure<RabbitMqOptions>(options);
        services.AddSingleton<IRabbitMqProvider, RabbitMqProvider>();
    }

    /// <summary>
    /// Consol 服务注入
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="tags"></param>
    public static void ConsulRegist(this IConfiguration configuration, string[] tags)
    {
        ConsulHandel.ConsulRegist(new DataConCore.Handels.HandelDto.ConsulSetting
        {
            ConsulService = "http://localhost:8500/",
            Datacenter = "dc1",
            ServerName = "DataConApi",
            Ip = configuration["ip"],
            Port = int.Parse(configuration["port"]),
            HealthPath = "healthz",
            Tags = tags
        });
    }

    public static IHost UseConsulRegist(this IHost host, IConfiguration configuration)
    {
        // 获取主机生命周期管理接口
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

        var options = configuration.GetSection("Consul");
        var optionsDto = options.Get<ConsulOptions>();
        var client = ConsulRegister.Init(optionsDto);

        //应用程序终止时,注销服务
        lifetime.ApplicationStopping.Register(() =>
        {
            client.Agent.ServiceDeregister($"service:{optionsDto.ServicePath}:{optionsDto.ServicePort}").Wait();
        });

        return host;
    }
}
