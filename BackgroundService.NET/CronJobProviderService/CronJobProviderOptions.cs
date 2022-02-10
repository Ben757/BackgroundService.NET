namespace BackgroundService.NET.CronJobProviderService;

public record CronJobProviderOptions
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? AssemblyProbeDirectory { get; set; }
}