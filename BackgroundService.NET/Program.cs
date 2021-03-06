using BackgroundService.NET;
using BackgroundService.NET.CronHostService;
using BackgroundService.NET.CronJobProviderService;
using NLog;
using NLog.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .UseWindowsService()
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

    services.AddSingleton<ILoggerFactory>(CreateNLogFactory(context.Configuration.GetSection("NLog")));
}

ILoggerFactory CreateNLogFactory(IConfigurationSection nLogConfiguration)
{
    LogManager.Configuration = new NLogLoggingConfiguration(nLogConfiguration);
    return new NLogLoggerFactory(new NLogLoggerProvider(new NLogProviderOptions(), LogManager.LogFactory));
}