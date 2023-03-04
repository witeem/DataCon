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
}
