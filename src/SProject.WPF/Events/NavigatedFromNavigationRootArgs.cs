using SProject.WPF.Abstractions;

namespace SProject.WPF.Models;

public class NavigatedFromNavigationRootArgs
{
    public required IServiceProvider ServiceProvider { get; init; }
    public required INavigationRoot NavigationRoot { get; init; }
}