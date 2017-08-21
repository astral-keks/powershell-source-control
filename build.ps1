$src = "$PSScriptRoot\src"
$package = "$PSScriptRoot\package"
$artifact = "$PSScriptRoot\artifact\AstralKeks.SourceControl"

# Build binaries
if (Test-Path $artifact) { Remove-Item $artifact -Recurse }
if (Test-Path $package) { Remove-Item $package -Recurse }
dotnet restore $src\Command\SourceControl.Command.csproj
dotnet build $src\Command\SourceControl.Command.csproj --configuration Release -o $artifact