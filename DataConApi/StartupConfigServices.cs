using DataConCore.Handels;

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
