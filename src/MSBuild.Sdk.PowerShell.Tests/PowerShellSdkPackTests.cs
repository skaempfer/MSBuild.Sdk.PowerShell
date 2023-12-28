using System.IO.Compression;

using Microsoft.Build.Utilities.ProjectCreation;

namespace MSBuild.Sdk.PowerShell.Tests;
public class PowerShellSdkPackTests : SdkTests
{
  [Fact]
  public void CreatesPowerShellGetComliantNugetPackage()
  {
    // Arrange
    const string packageId = "MyPackage";
    const string version = "1.0.0";
    ProjectCreator projectCreator = this.CreatePowerShellProject()
        .Property("PackageOutputPath", Path.Combine(this.TestRootPath, "bin"))
        .Property("PackageId", packageId)
        .Property("Version", version)
        .Save();

    // Act
    projectCreator.TryBuild(restore: true, target: "Pack", out bool success, out BuildOutput output);

    // Assert
    Assert.True(success, output.GetConsoleLog());

    string packagePath = Path.Combine(this.TestRootPath, "bin", $"{packageId}.{version}.nupkg");
    Assert.True(File.Exists(packagePath));

    using Stream nugetPackageStream = new FileStream(packagePath, FileMode.Open);
    ZipArchive nugetPackage = new ZipArchive(nugetPackageStream, ZipArchiveMode.Read);
    Assert.Contains(nugetPackage.Entries, e => e.FullName == $"{packageId}.nuspec");
    Assert.Contains(nugetPackage.Entries, e => e.FullName == $"{packageId}.psd1");
    Assert.Contains(nugetPackage.Entries, e => e.FullName == $"{this.TestProjectName}.dll");
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
