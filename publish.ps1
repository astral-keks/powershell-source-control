$artifact = "$PSScriptRoot\artifact"
$bin = "$artifact\bin"
$module = "$bin\SourceControl"

$apiKey = Read-Host -Prompt 'Enter API key'
Publish-Module -Path $module -NuGetApiKey $apiKey