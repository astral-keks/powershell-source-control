$version = "0.2.5"

$src = "$PSScriptRoot\src"
$artifact = "$PSScriptRoot\artifact"

$bin = "$artifact\bin"

$module = "$bin\SourceControl" 

$manifest = "$module\SourceControl.psd1"

# Build binaries
if (Test-Path $artifact) { Remove-Item $artifact -Recurse }
dotnet restore $src\Command\SourceControl.Command.csproj
dotnet build $src\Command\SourceControl.Command.csproj --configuration Release -o $module --version-suffix $version

# Fix manifest version
(Get-Content $manifest).replace("ModuleVersion = '0.0.0'", "ModuleVersion = '$version'") | Set-Content $manifest