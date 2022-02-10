using BackgroundService.NET.CronJob;

namespace BackgroundService.NET.FailingPlugin;

// ReSharper disable once UnusedType.Global
public class RandomFailingJob : ICronJob
{
    private readonly Random random = new();

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var randomDelay = random.Next(10);

        await Task.Delay(TimeSpan.FromMilliseconds(100 * randomDelay), cancellationToken);

        var shouldFail = random.Next(100) < 50;

        if (shouldFail)
            throw new InvalidOperationException("Failed randomly");
        
        Console.WriteLine($"{DateTimeOffset.Now}: Job completed successfully");
    }
}