using BackgroundService.NET.CronJob;
using BackgroundService.NET.PluginProjectReference;

namespace BackgroundService.NET.PluginWithProjectReference;

public class PluginWithProjectReferenceJob : ICronJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        new PluginHelper().Print();
        
        return Task.CompletedTask;
    }
}