using System.Reflection;
using BackgroundService.NET.CronJob;

namespace BackgroundService.NET.CronJobProviderService;

public class CronJobProvider : ICronJobProvider
{
    private ICronJob? cronJob;
    
    public ICronJob GetCronJob()
    {
        if (cronJob == null)
        {
            var pluginAssembly = LoadPluginAssembly();

            cronJob = LoadCronJobFromAssembly(pluginAssembly);
        }

        return cronJob;
    }

    private static Assembly? LoadPluginAssembly()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "CronJob.dll");

        try
        {
            var pluginLoadContext = new PluginLoadContext(path);

            return pluginLoadContext.LoadAssembly();
        }
        catch (InvalidOperationException)
        {
            throw new PluginNotFoundException($"Could not find plugin assembly at '{path}'");
        }
    }

    private static ICronJob LoadCronJobFromAssembly(Assembly? assembly)
    {
        if (assembly == null)
            throw new PluginNotFoundException("Could not find plugin assembly");
        
        foreach (var type in assembly.GetTypes())
        {
            if (!typeof(ICronJob).IsAssignableFrom(type)) continue;

            if (Activator.CreateInstance(type) is ICronJob result) return result;
        }

        throw new PluginNotFoundException(
            $"Could not find any implementation of {nameof(ICronJob)} in the plugin assembly at {assembly.Location}");
    }
}