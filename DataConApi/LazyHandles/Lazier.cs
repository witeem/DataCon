namespace DataConApi.LazyHandles
{
    public class Lazier<T> : Lazy<T> where T : class
    {
        public Lazier(IServiceProvider serviceProvider) : base(serviceProvider.GetService<T>) { }
    }
}
