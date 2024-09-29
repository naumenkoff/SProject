using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Hosting;
using SProject.WPF.Abstractions;

namespace SProject.WPF.HostedServices;

internal sealed class StartupService<TMainViewModel>(IMainViewOf<TMainViewModel> mainView)
    : BackgroundService where TMainViewModel : ObservableObject
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await mainView.ShowAsync(stoppingToken).ConfigureAwait(false);
    }
}