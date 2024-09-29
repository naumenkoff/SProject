using System.Diagnostics.CodeAnalysis;

namespace SProject.DependencyInjection;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IServiceScopeFactory<out T> where T : class
{
    IServiceScope<T> CreateScope();
}