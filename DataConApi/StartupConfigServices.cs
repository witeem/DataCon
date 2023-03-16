using DataCon.IApplication;
using DataCon.IRepositories;
using DataCon.Repositories;
using DataConCore;
using DataConCore.Handels;

public static class ServicesProvider
{
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
            .WithSingletonLifetime()
            .AddClasses(classes => classes.AssignableTo<IAppCore>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime();
        });
    }

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
