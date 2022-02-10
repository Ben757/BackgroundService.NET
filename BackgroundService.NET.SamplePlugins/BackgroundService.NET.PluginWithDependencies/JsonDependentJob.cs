using BackgroundService.NET.CronJob;
using Newtonsoft.Json;

namespace BackgroundService.NET.PluginWithDependencies;

// ReSharper disable once UnusedType.Global
public class JsonDependentJob : ICronJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var sampleObject = new
        {
            Id = Guid.NewGuid(),
            SomeProperty = new
            {
                Text = "HelloWorld"
            }
        };

        var json = JsonConvert.SerializeObject(sampleObject);
        
        Console.WriteLine($"{DateTimeOffset.Now}: {json}");
        
        return Task.CompletedTask;
    }
}