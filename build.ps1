$version = "0.1.3"

$src = "$PSScriptRoot\src"
$package = "$PSScriptRoot\package"
$artifact = "$PSScriptRoot\artifact\AstralKeks.SourceControl"

$manifest = "$artifact\AstralKeks.SourceControl.psd1"

# Build binaries
if (Test-Path $artifact) { Remove-Item $artifact -Recurse }
if (Test-Path $package) { Remove-Item $package -Recurse }
dotnet restore $src\Command\SourceControl.Command.csproj
dotnet build $src\Command\SourceControl.Command.csproj --configuration Release -o $artifact --version-suffix $version

# Fix manifest version
(Get-Content $manifest).replace("ModuleVersion = '0.0.1'", "ModuleVersion = '$version'") | Set-Content $manifest