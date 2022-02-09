namespace BackgroundService.NET.CronHostService;

public interface ICronWaiter
{
    Task WaitAsync(TimeSpan waitTime, CancellationToken cancellationToken);
}