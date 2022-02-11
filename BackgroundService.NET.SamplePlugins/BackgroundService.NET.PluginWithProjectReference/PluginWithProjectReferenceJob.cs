using BackgroundService.NET.CronJob;
using BackgroundService.NET.PluginProjectReference;

namespace BackgroundService.NET.PluginWithProjectReference;

public class PluginWithProjectReferenceJob : ICronJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await PluginHelper.PrintAsync();
    }
}