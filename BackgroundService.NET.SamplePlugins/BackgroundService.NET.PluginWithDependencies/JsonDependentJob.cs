using BackgroundService.NET.CronJob;
using Newtonsoft.Json;

namespace BackgroundService.NET.PluginWithDependencies;

// ReSharper disable once UnusedType.Global
public class JsonDependentJob : ICronJob
{
    private const string FilePath = @"C:\ProgramData\BackgroundService.NET\sampleoutput.txt";
    
    public async Task ExecuteAsync(CancellationToken cancellationToken)
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
        
        await File.AppendAllLinesAsync(FilePath, new[]{$"{DateTimeOffset.Now}: {json}"}, cancellationToken);
    }
}