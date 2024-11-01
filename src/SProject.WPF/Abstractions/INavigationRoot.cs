using System.Diagnostics.CodeAnalysis;

namespace SProject.WPF.Abstractions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface INavigationRoot
{
    void EnsureNavigationSelected(Type type);

    void Navigate(Type type);
}