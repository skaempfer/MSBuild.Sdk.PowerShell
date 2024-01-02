# MSBuild.Sdk.PowerShell

Simplifies the workflow of creating and bundling binary PowerShell modules. This package provides and MSBuild-centric module creation approach that assists you in the following tasks:

- automatic creation of a module manifest file (.psd1) based on the data provided in your project file (.csproj)
- creating a Nuget package that is ready to be consumed by *PowerShellGet*

## Installation

In the .csproj of your PowerShell project replace the value of the 'Sdk' attribute with 'MSBuild.PowerShell.Sdk' and the specific version:

```xml
<Project Sdk="MSBuild.Sdk.PowerShell/0.1.0">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>

  <PropertyGroup>
    <ModuleId>080462d7-f4bd-4135-aade-6d0d9b609feb</ModuleId>
    <Copyright>John Doe</Copyright>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="PowerShellStandard.Library" Version="5.1.1" PrivateAssets="All" />
  </ItemGroup>

</Project>
```

For more information on how to use custom MSBuild project sdks see the [official Microsoft documentation](https://learn.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk?view=vs-2022).


## General Usage

### Providing Package Metadata

### Generating a PowerShell Module NuGet Package 
