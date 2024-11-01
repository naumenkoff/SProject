using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SProject.WPF.Abstractions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IView
{
    void Initialize(IServiceProvider serviceProvider);
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IViewOf<out TVm> : IView where TVm : ObservableObject
{
    TVm ViewModel { get; }
}