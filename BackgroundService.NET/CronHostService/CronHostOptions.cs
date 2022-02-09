namespace BackgroundService.NET.CronHostService;

public record CronHostOptions
{
    public string CronString { get; set; } = string.Empty;
}