namespace SProject.DependencyInjection;

public interface IServiceScopeFactory<out T> where T : class
{
    IServiceScope<T> CreateScope();
}