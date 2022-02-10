using BackgroundService.NET.CronJob;

namespace BackgroundService.NET.HelloWorldPlugin;

// ReSharper disable once UnusedType.Global
public class HelloWorldJob : ICronJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"Hello World at {DateTimeOffset.Now}");
        
        return Task.CompletedTask;
    }
}