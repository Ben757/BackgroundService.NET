using BackgroundService.NET;
using BackgroundService.NET.CronHostService;
using BackgroundService.NET.CronJobProviderService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

await host.RunAsync();

void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddHostedService<WorkerService>();

    services.AddTransient<ICronHost, CronHost>();
    services.AddTransient<ICronWaiter, CronWaiter>();
    services.AddSingleton<ICronJobProvider, CronJobProvider>();

    services.Configure<CronHostOptions>(context.Configuration.GetSection(nameof(CronHostOptions)));
    services.Configure<CronJobProviderOptions>(context.Configuration.GetSection(nameof(CronJobProviderOptions)));
}