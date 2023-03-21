using DataCon.IApplication;
using DataCon.IRepositories;
using DataCon.Repositories;
using DataConApi;
using DataConCore;
using DataConCore.Handels;
using DataConCore.Handels.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;

public static class ServicesProvider
{
    /// <summary>
    /// Application服务注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    public static void AddStartupConfigServices(this IServiceCollection services, List<Type> types)
    {
        //List<Type> types = AppDomain.CurrentDomain
        //                    .GetAssemblies()
        //                    .SelectMany(m=>m.GetTypes())
        //                    .Where(t=> iType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
        //                    .ToList();

        //foreach (Type type in types)
        //{
        //    Type[] interfaces = type.GetInterfaces();
        //    interfaces.ToList().ForEach(s =>
        //    {
        //        services.AddScoped(s, type);
        //    });
        //}

        services.Scan(scan =>
        {
            scan.FromAssembliesOf(types.ToArray())
            .AddClasses(classes => classes.AssignableTo<IAppServers>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<IAppCore>())
            .AsImplementedInterfaces()
            .WithScopedLifetime();
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
    /// RabbitMq 生产者服务注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void MqProducerRegist(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("RabbitMqOptions");
        services.Configure<RabbitMqOptions>(options);
        services.AddScoped<IRabbitMqProvider, RabbitMqProvider>();
    }

    /// <summary>
    /// RabbitMq 消费者服务注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void MqConsumerRegist(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("RabbitMqOptions").Get<RabbitMqOptions>();
        if (options.Hosts?.Count > 0)
        {
            services.AddSingleton(typeof(IRabbitMqProducer), (serviceProvider) =>
            {
                var mqProducer = new RabbitMqProducer(JsonConvert.SerializeObject(options));
                return mqProducer;
            });
        }
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
}
