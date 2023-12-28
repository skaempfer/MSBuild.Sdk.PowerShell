Remove-Item $PSScriptRoot\nuget\ -Recurse -ErrorAction Ignore

& dotnet pack -c Release $PSScriptRoot\..\src\MSBuild.Sdk.PowerShell\MSBuild.Sdk.PowerShell.csproj -o $PSScriptRoot\nuget
