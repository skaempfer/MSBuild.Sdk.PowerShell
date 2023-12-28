using Microsoft.Build.Utilities.ProjectCreation;

namespace MSBuild.Sdk.PowerShell.Tests;

public class CreateModuleManifestTests : MSBuildTestBase
{
  [Fact]
  public void CreatesModuleManifest()
  {
    // Arrange
    BuildEngine buildEngine = BuildEngine.Create();

    string author = "John Doe";
    string[] cmdlets = new string[] { "Add-Item", "Get-Item" };
    string companyName = "Acme";
    string copyright = "(c) Acme";
    string description = "A simple test";
    string moduleId = "A488715D-9F1C-41BF-9872-451610416D91";
    string moduleVersion = "1.0.0";
    string[] tags = new string[] { "Test", "Test2" };

    CreateModuleManifest task = new CreateModuleManifest
    {
      BuildEngine = buildEngine,
      Author = author,
      CmdletsToExport = cmdlets,
      CompanyName = companyName,
      Copyright = copyright,
      Description = description,
      ModuleId = moduleId,
      ModuleVersion = moduleVersion,
      Tags = tags,
    };

    // Act
    bool success = task.Execute();

    // Assert
    Assert.True(success, buildEngine.GetConsoleLog());
    string expected = $@"@{{
    RootModule = ''
    ModuleVersion = '{moduleVersion}'
    CompatiblePSEditions = @()
    GUID = '{moduleId}'
    Author = '{author}'
    CompanyName = '{companyName}'
    Copyright = '{copyright}'
    Description = '{description}'
    PowerShellVersion = ''
    PowerShellHostName = ''
    PowerShellHostVersion = ''
    DotNetFrameworkVersion = ''
    ClrVersion = ''
    ProcessorArchitecture = ''
    RequiredModules = @()
    RequiredAssemblies = @()
    ScriptsToProcess = @()
    TypesToProcess = @()
    FormatsToProcess = @()
    NestedModules = @()
    FunctionsToExport = @()
    CmdletsToExport = @('{cmdlets[0]}','{cmdlets[1]}')
    VariablesToExport = '*'
    AliasesToExport = @()
    DscResourcesToExport = @()
    ModuleList = @()
    FileList = @()
    PrivateData = @{{
        PSData = @{{
            Tags = @('{tags[0]}','{tags[1]}')
            LicenseUri = ''
            ProjectUri = ''
            IconUri = ''
            ReleaseNotes = ''
            Prerelease = ''
            RequireLicenseAcceptance = $false
            ExternalModuleDependencies = @()
        }}
    }}
    HelpInfoURI = ''
    DefaultCommandPrefix = ''
}}
";
    Assert.Equal(expected, task.ManifestContent.ItemSpec);
  }
}
