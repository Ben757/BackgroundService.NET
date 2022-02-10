using BackgroundService.NET.CronJobProviderService;
using Cronos;
using Microsoft.Extensions.Options;

namespace BackgroundService.NET.CronHostService;

public class CronHost : ICronHost
{
    private readonly ICronJobProvider jobProvider;
    private readonly ILogger<CronHost> logger;
    private readonly IOptionsMonitor<CronHostOptions> optionsMonitor;
    private readonly ICronWaiter waiter;

    private CronHostOptions CronHostOptions => optionsMonitor.CurrentValue;

    public CronHost(ICronJobProvider jobProvider, ICronWaiter waiter, ILogger<CronHost> logger, IOptionsMonitor<CronHostOptions> optionsMonitor)
    {
        this.jobProvider = jobProvider;
        this.logger = logger;
        this.optionsMonitor = optionsMonitor;
        this.waiter = waiter;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Starting cron host");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var cronJob = jobProvider.GetCronJob();
                
                var cronExpression = CronExpression.Parse(CronHostOptions.CronString, CronFormat.IncludeSeconds);

                var now = DateTime.UtcNow;
                var waitTime = cronExpression.GetNextOccurrence(now) - now;

                if (waitTime.HasValue && waitTime.Value > TimeSpan.Zero)
                {
                    await waiter.WaitAsync(waitTime.Value, cancellationToken);
                }
                else
                {
                    logger.LogWarning("Calculated invalid wait time '{WaitTime}'", waitTime);
                }

                await cronJob.ExecuteAsync(cancellationToken);
            }
            catch (PluginNotFoundException pluginNotFoundException)
            {
                logger.LogCritical(pluginNotFoundException, "Could not find plugin");
            }
            catch (OperationCanceledException)
            {
                logger.LogDebug("Cron host was stopped");
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Unknown exception thrown in cron job. Retry at next cron due time");
            }
        }
        
        logger.LogDebug("Stopped cron host");
    }
}