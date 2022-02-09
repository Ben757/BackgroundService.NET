namespace BackgroundService.NET.CronHostService;

public interface ICronHost
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}