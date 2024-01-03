# MSBuild.Sdk.PowerShell

Simplifies the workflow of creating and bundling binary PowerShell modules. This package provides and MSBuild-centric module creation approach that assists you in the following tasks:

- automatic creation of a module manifest file (.psd1) based on the data provided in your project file (.csproj)
- creating a Nuget package that is ready to be consumed by *PowerShellGet*

[![Nuget](https://img.shields.io/nuget/v/MSBuild.Sdk.PowerShell?style=flat)](https://www.nuget.org/packages/MSBuild.Sdk.PowerShell)
[![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/skaempfer/MSBuild.Sdk.PowerShell/actions/workflows/test.yml?style=flat&label=tests)](https://github.com/skaempfer/MSBuild.Sdk.PowerShell/actions/workflows/test.yml)

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

### Customization

Available MSBuild Properties

- `GenerateModuleManifest` (default: True): Set this to `False` if you are providing your own `.psd1` file 
