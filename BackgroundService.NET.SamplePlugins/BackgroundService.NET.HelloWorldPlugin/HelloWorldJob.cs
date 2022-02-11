using BackgroundService.NET.CronJob;

namespace BackgroundService.NET.HelloWorldPlugin;

// ReSharper disable once UnusedType.Global
public class HelloWorldJob : ICronJob
{
    private const string FilePath = @"C:\ProgramData\BackgroundService.NET\sampleoutput.txt";
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await File.AppendAllLinesAsync(FilePath, new[]{$"Hello World at {DateTimeOffset.Now}"}, cancellationToken);
    }
}