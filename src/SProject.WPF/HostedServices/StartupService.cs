using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Hosting;
using SProject.WPF.Abstractions;

namespace SProject.WPF.HostedServices;

internal sealed class StartupService<TVm>(IMainViewOf<TVm> mainView) : BackgroundService where TVm : ObservableObject
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await mainView.ShowAsync(stoppingToken).ConfigureAwait(false);
    }
}