using System.Reflection;
using System.Runtime.Loader;

namespace BackgroundService.NET.CronJobProviderService;

public class PluginLoadContext : AssemblyLoadContext
{
    private readonly string pluginPath;
    private readonly AssemblyDependencyResolver resolver;

    public PluginLoadContext(string pluginPath)
    {
        this.pluginPath = pluginPath;
        resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
        
        return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return libraryPath != null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
    }

    public Assembly? LoadAssembly()
    {
        return Load(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
    }
}