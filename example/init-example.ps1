Remove-Item $PSScriptRoot\nuget\ -Recurse -ErrorAction Ignore

& dotnet pack -c Release $PSScriptRoot\..\src\MSBuild.Sdk.PowerShell\MSBuild.Sdk.PowerShell.csproj -o $PSScriptRoot\nuget

$repositoryPath = "$PSScriptRoot/powershellget-repository"

if(-Not (Test-Path -Path $repositoryPath -PathType Container)) {
  New-Item -Path $repositoryPath -ItemType Directory
}

Register-PSRepository -Name ExampleLocal -SourceLocation $repositoryPath -PublishLocation $repositoryPath -InstallationPolicy Trusted
