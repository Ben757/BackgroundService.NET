namespace BackgroundService.NET.CronHostService;

public class CronWaiter : ICronWaiter
{
    public async Task WaitAsync(TimeSpan waitTime, CancellationToken cancellationToken)
    {
        await Task.Delay(waitTime, cancellationToken);
    }
}