namespace BackgroundService.NET.PluginProjectReference;

public class PluginHelper
{
    public void Print()
    {
        Console.WriteLine($"{DateTimeOffset.Now} This is printed from the reference project");
    }
}