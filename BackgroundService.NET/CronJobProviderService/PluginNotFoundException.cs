using System.Runtime.Serialization;

namespace BackgroundService.NET.CronJobProviderService;

public class PluginNotFoundException : Exception
{
    public PluginNotFoundException()
    {
    }

    protected PluginNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public PluginNotFoundException(string? message) : base(message)
    {
    }

    public PluginNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}