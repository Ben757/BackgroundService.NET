using BackgroundService.NET;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

await host.RunAsync();

void ConfigureServices(IServiceCollection services)
{
    services.AddHostedService<WorkerService>(); 
}