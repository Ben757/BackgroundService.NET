namespace BackgroundService.NET.CronJob;

public interface ICronJob
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}