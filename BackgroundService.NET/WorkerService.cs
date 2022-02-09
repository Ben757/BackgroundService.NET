using BackgroundService.NET.CronHostService;

namespace BackgroundService.NET;
using Microsoft.Extensions.Hosting;

public class WorkerService : BackgroundService
{
    private readonly ILogger<WorkerService> logger;
    private readonly ICronHost cronHost;

    public WorkerService(ICronHost cronHost, ILogger<WorkerService> logger)
    {
        this.logger = logger;
        this.cronHost = cronHost;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await cronHost.ExecuteAsync(stoppingToken);
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Unknown error during execution of cron host. This might be a bug");
        }
    }
}