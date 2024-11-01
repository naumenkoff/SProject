using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SProject.WPF.Services;

public sealed class ShutdownService(IHost host, ILogger<ShutdownService> logger)
{
    public void Shutdown()
    {
        try
        {
            host.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        catch (Exception exception)
        {
            OnStopFailed(exception);
        }
        finally
        {
            host.Dispose();
        }
    }

    private void OnStopFailed(Exception exception)
    {
        logger.LogCritical(exception, "Host failed to stop");
    }
}