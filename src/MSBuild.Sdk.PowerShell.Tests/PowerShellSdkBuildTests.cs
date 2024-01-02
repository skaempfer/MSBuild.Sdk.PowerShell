using Microsoft.Build.Utilities.ProjectCreation;

namespace MSBuild.Sdk.PowerShell.Tests;
public class PowerShellSdkBuildTests : SdkTests
{
  public PowerShellSdkBuildTests()
  {
    Directory.CreateDirectory(this.TestRootPath);
    Environment.CurrentDirectory = this.TestRootPath;
  }

  [Fact]
  public void BuildPowerShellSdkProject()
  {
    // Arrange
    ProjectCreator projectCreator = this.CreatePowerShellProject().Save();

    // Act
    projectCreator.TryBuild(restore: true, out bool success, out BuildOutput output);

    // Assert
    Assert.True(success, output.GetConsoleLog());
    string outputPath = Path.Combine(this.TestRootPath, "bin", "Debug", "net5.0");
    Assert.True(File.Exists(Path.Combine(outputPath, $"{this.TestProjectName}.dll")));
    Assert.True(File.Exists(Path.Combine(outputPath, $"{this.TestProjectName}.psd1")));
  }

  [Fact]
  public void NoDynamicManifestGeneration()
  {
    // Arrange
    ProjectCreator projectCreator = this.CreatePowerShellProject()
        .Property("GenerateModuleManifest", "false")
        .Save();

    // Act
    projectCreator.TryBuild(restore: true, out bool success, out BuildOutput output);

    // Assert
    Assert.True(success, output.GetConsoleLog());
    string outputPath = Path.Combine(this.TestRootPath, "bin", "Debug", "net5.0");
    Assert.True(File.Exists(Path.Combine(outputPath, $"{this.TestProjectName}.dll")));
    DirectoryInfo outputDirectory = new DirectoryInfo(outputPath);
    Assert.Empty(outputDirectory.GetFiles("*.psd1"));
  }

  public static IEnumerable<object[]> MandatoryManifestProperties =>
      new List<object[]>
      {
                new object[] { "Copyright" },
                new object[] { "ModuleId" },
      };

  [Theory]
  [MemberData(nameof(MandatoryManifestProperties))]
  public void BuildFailsWhenMandatoryManifestPropertiesAreNotSupplied(string property)
  {
    // Arrange
    ProjectCreator projectCreator = this.CreatePowerShellProject()
        .Property(property, string.Empty)
        .Save();

    // Act
    projectCreator.TryBuild(restore: true, out bool success, out BuildOutput output);

    // Assert
    Assert.False(success, output.GetConsoleLog());
  }

  [Theory]
  [MemberData(nameof(MandatoryManifestProperties))]
  public void ManifestPropertiesAreNotMandatoryWhenManifestIsUserProvided(string property)
  {
    // Arrange
    ProjectCreator projectCreator = this.CreatePowerShellProject()
        .Property("GenerateModuleManifest", "false")
        .Property(property, string.Empty)
        .Save();

    // Act
    projectCreator.TryBuild(restore: true, out bool success, out BuildOutput output);

    // Assert
    Assert.True(success);
  }

  [Fact]
  public void UsesExpectedMsBuildPropertiesForModuleCreation()
  {
    // Arrange
    string formatFile = "Type1.format.ps1xml";
    this.CreateFiles(Path.Combine(this.TestRootPath, "bin", "Debug", "net5.0"), formatFile);

    ProjectCreator projectCreator = this.CreatePowerShellProject()
        .Property("CmdletsToExport", "Add-Item;Get-Item")
        .Property("PackageTags", "Tag1 Tag2 Tag3")
        .Save();

    // Act
    projectCreator.TryBuild(target: "_CreateModuleManifest", out bool success, out BuildOutput output);

    // Assert
    Assert.True(success, output.GetConsoleLog());
    string actualManifest = output.Messages.Single(m => m.StartsWith("@{"));
    string expectedManifest = $@"@{{
    RootModule = 'PowerShellTestModule.dll'
    ModuleVersion = '1.0.0'
    CompatiblePSEditions = @()
    GUID = '2D4A1A7C-FDA1-423D-A528-6070A7314776'
    Author = 'John Doe'
    CompanyName = 'ACME'
    Copyright = '(c) ACME'
    Description = 'A test module'
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
    FormatsToProcess = @('{formatFile}')
    NestedModules = @()
    FunctionsToExport = @()
    CmdletsToExport = @('Add-Item','Get-Item')
    VariablesToExport = '*'
    AliasesToExport = @()
    DscResourcesToExport = @()
    ModuleList = @()
    FileList = @()
    PrivateData = @{{
        PSData = @{{
            Tags = @('Tag1','Tag2','Tag3','PSModule')
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

    Assert.Equal(expectedManifest, actualManifest);
  }

  protected ProjectCreator CreatePowerShellProject()
  {
    string thisAssemblyPath = Path.GetDirectoryName(typeof(PowerShellSdkBuildTests).Assembly.Location)
        ?? throw new InvalidOperationException($"{nameof(thisAssemblyPath)} is null");

    return ProjectCreator.Create(
            path: this.TestProjectPath)
        .Property("PowerShellTaskAssembly", Path.Combine(thisAssemblyPath, "MsBuild.Sdk.PowerShell.dll"))
        .Property("TargetFramework", "net5.0")
        .PropertyGroup()
            .Property("Authors", "John Doe")
            .Property("Company", "ACME")
            .Property("Copyright", "(c) ACME")
            .Property("Description", "A test module")
            .Property("ModuleId", "2D4A1A7C-FDA1-423D-A528-6070A7314776")
            .Property("Version", "1.0.0")
        .Import(Path.Combine(thisAssemblyPath, "Sdk", "Sdk.props"))
        .Import(Path.Combine(thisAssemblyPath, "Sdk", "Sdk.targets"));
  }
}
