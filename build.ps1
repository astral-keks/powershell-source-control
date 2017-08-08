$src = "$PSScriptRoot\src"
$artifact = "$PSScriptRoot\artifact"

# Build binaries
if (Test-Path $artifact) { Remove-Item $artifact -Recurse }
dotnet restore $src\Command\SourceControl.Command.csproj
dotnet build $src\Command\SourceControl.Command.csproj --configuration Release -o  $artifact