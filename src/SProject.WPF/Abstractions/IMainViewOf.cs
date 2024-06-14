using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SProject.WPF.Abstractions;

public interface IMainViewOf<[SuppressMessage("ReSharper", "UnusedTypeParameter")] TVm> : IViewOf<TVm> where TVm : ObservableObject
{
    Task ShowAsync(CancellationToken cancellationToken = default);
}