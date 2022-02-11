namespace BackgroundService.NET.PluginProjectReference;

public static class PluginHelper
{
    private const string FilePath = @"C:\ProgramData\BackgroundService.NET\sampleoutput.txt";
    public static async Task PrintAsync()
    {
        await File.AppendAllLinesAsync(FilePath, new[]{$"{DateTimeOffset.Now} This is printed from the reference project"});
    }
}