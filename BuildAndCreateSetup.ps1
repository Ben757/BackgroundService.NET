$pluginProjectPath=$args[0]
$pluginOutputDir= -join($args[1],"\*")

Write-Host "Building Backgroundservice"

dotnet publish -c Release "BackgroundService.NET\\BackgroundService.NET.csproj"


Write-Host "Building project $pluginProjectPath"

dotnet publish -c Release $pluginProjectPath


Write-Host "Copying $pluginOutputDir to Plugins dir"

New-Item -Path "BackgroundService.NET\publish" -Name "Plugins" -ItemType "directory"
Copy-Item -Path $pluginOutputDir -Destination "BackgroundService.NET\publish\Plugins" -Recurse

Write-Host "Creating Setup"

MSBuild.exe -p:Configuration=Release "BackgroundService.NET.Setup\\BackgroundService.NET.Setup.wixproj"

Write-Host "Finished"