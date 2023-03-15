using DataCon.IRepositories;
using DataCon.IRepositories.WxUser;
using DataCon.Repositories;
using DataCon.Repositories.WxUser;
using DataConCore.Handels;
using DataConCore.TableEntitys;
using Scrutor;

public static class ServicesProvider
{
    public static void AddStartupConfigServices(this IServiceCollection services, Type iType)
    {
        List<Type> types = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(m=>m.GetTypes())
                            .Where(t=> iType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                            .ToList();

        foreach (Type type in types)
        {
            Type[] interfaces = type.GetInterfaces();
            interfaces.ToList().ForEach(s =>
            {
                services.AddScoped(s, type);
            });
        }
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        Type iType = typeof(IBaseRepositories<>);
        List<Type> types = new List<Type> { typeof(BaseRepositories<>) };
        services.Scan(scan => {
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
            ServerName = "DataConApi",
            Ip = configuration["ip"],
            Port = int.Parse(configuration["port"]),
            HealthPath = "healthz",
            Tags = tags
        });
    }
}
