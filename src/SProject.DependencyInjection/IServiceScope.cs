namespace SProject.DependencyInjection;

public interface IServiceScope<out T> : IDisposable where T : class
{
    T GetRequiredService();

    T GetRequiredKeyedService(object? serviceKey);

    T? GetService();

    T? GetKeyedService(object? serviceKey);

    IEnumerable<T> GetServices();
}