using CommunityToolkit.Mvvm.ComponentModel;

namespace SProject.WPF.Abstractions;

public interface IMainViewOf<TVm> : IViewOf<TVm> where TVm : ObservableObject
{
    Task ShowAsync(CancellationToken cancellationToken = default);
}