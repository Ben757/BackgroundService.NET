# BackgroundService.NET

BackgroundService.NET provides a basic windows service that executes scheduled tasks based on a plugin concept.
It is a stable and undestroyable cron worker that executes a single task found in a plugin. It is written in .NET 6. The repository provides a simple 
[Wix 3.11](https://wixtoolset.org/releases/) setup to install the service including the plugins. A Powershell script is provided to simplify the process of creating the MSI.

## Getting started

This project is meant to be used as a basic service. So you'll have to implement the functionallity on your own by creating a plugin. 
BackgroundService.NET currently can only handle one plugin with a single job.
To get started, check out the repository and have a look at the sample plugins.

### Implement a Plugin

To implement your own plugin, you'll have to implement the interface `ICronJob`. It has a single public funtion that is called by the service at the cron intervals.
```csharp
public interface ICronJob
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
```
Create a new project and reference the project `BackgroundService.NET.CronJob.csproj`. To make it useable as a plugin, you'll have to adapt your `.csproj` file.
Copy the contents of the sample plugins and adapt the path to `BackgroundService.NET.CronJob.csproj`. The steps to create a plugin system in .NET can be found in the [documentation](https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support#simple-plugin-with-no-dependencies).

**Note** that the assembly name is changed and set to *CronJob*. This means, that the project's output assembly will be named *CronJob.dll*. This is needed for the serivce to find the plugin at runtime.

Create a class and let it implement `ICronJob`. The following example is taken from the HelloWorld sample plugin.

```csharp
public class HelloWorldJob : ICronJob
{
    private const string FilePath = @"C:\ProgramData\BackgroundService.NET\sampleoutput.txt";
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await File.AppendAllLinesAsync(FilePath, new[]{$"Hello World at {DateTimeOffset.Now}"}, cancellationToken);
    }
}
```

That's all you'll have to do. You can do anything you want in `ExecuteAsync`. The background service will even handle any exceptions that bubble up from this method. 
They are logged and the service will retry it at the next cron time. 

### Debugging your plugins

To debug your plugins within the service, the BackgroundService.NET has a DEBUG switch that allows setting the plugin probe path. 
This can be done via the [app's configuration](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration). 
The path is set in `appsettings.Development.json`. Currently it is set to one of the sample plugins. Change it according to your needs.

Happy debugging!

### Cron times

The cron interval is set in the `appsettings.json` file. Just change `CronHostOptions.CronString` according to your needs. 
The services uses [Cronos}(https://github.com/HangfireIO/Cronos) with enabled seconds to determine the schedule.

### Logging

BackgroundService.NET logs to file using [NLog](https://nlog-project.org/). It is configured in `appsettings.json` as well. Feel free to change the settings to your needs.

## The setup

This repository contains a Wix project to create a setup for your service. The setup automaticall installs the windows service and enables automatic start and restarts on failures.
The service will run as *LocalService* user.
The setup is meant to harvest your plugin's output to catch all assembly coming from referenced projects or NuGet packages. It uses `BackgroundService.NET\publish\Plugins` as harvest directory for the plugin.

The setup contains some configuration values for your service's appearance like its name and description. Those can be found at top of `Product.wxs`. Change them according to your needs.
You should also [change the GUIDs](https://www.firegiant.com/wix/tutorial/getting-started/the-files-inside/). 

### Using the Powershell script

To simplify setup creation, the repository provides the Powershell script `BuildAndCreateSetup.ps1`. This script publishes the service and your plugin. 
Afterward, it copies the plugins output to `BackgroundService.NET\publish\Plugins` in order to allow harvesting it by the Wix setup. Then, the setup is created.
It uses RELEASE build mode. The setup will be available at `BackgroundService.NET\BackgroundService.NET.Setup\bin\Release`.




