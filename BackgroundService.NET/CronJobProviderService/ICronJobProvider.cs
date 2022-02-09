using BackgroundService.NET.CronJob;

namespace BackgroundService.NET.CronJobProviderService;

public interface ICronJobProvider
{
    ICronJob GetCronJob();
}