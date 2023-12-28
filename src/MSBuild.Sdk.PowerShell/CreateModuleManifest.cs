using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Sdk.PowerShell;
public class CreateModuleManifest : Microsoft.Build.Utilities.Task
{
  [Required]
  public string Author { get; set; }

  [Required]
  public string CompanyName { get; set; }

  public string[] CmdletsToExport { get; set; } = new string[0];

  [Required]
  public string Copyright { get; set; }

  public string Description { get; set; }

  public string[] FormatsToProcess { get; set; } = new string[0];

  [Required]
  public string ModuleId { get; set; }

  [Required]
  public string ModuleVersion { get; set; }

  [Required]
  public string PackageId { get; set; }

  public string ProjectUri { get; set; }

  [Required]
  public string RootModule { get; set; }

  public string[] Tags { get; set; } = new string[0];

  [Output]
  public ITaskItem ManifestContent { get; private set; }

  public override bool Execute()
  {
    (string versionPrefix, string versionSuffix) = Split(this.ModuleVersion, includeSeparator: true);

    string manifestContent = $@"@{{
    RootModule = '{this.RootModule}'
    ModuleVersion = '{versionPrefix}'
    CompatiblePSEditions = @()
    GUID = '{this.ModuleId}'
    Author = '{this.Author}'
    CompanyName = '{this.CompanyName}'
    Copyright = '{this.Copyright}'
    Description = '{this.Description}'
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
    FormatsToProcess = {this.FormatsToProcess.ToPowerShellArray()}
    NestedModules = @()
    FunctionsToExport = @()
    CmdletsToExport = {this.CmdletsToExport.ToPowerShellArray()}
    VariablesToExport = '*'
    AliasesToExport = @()
    DscResourcesToExport = @()
    ModuleList = @()
    FileList = @()
    PrivateData = @{{
        PSData = @{{
            Tags = {this.Tags.ToPowerShellArray()}
            LicenseUri = ''
            ProjectUri = ''
            IconUri = ''
            ReleaseNotes = ''
            Prerelease = '{versionSuffix}'
            RequireLicenseAcceptance = $false
            ExternalModuleDependencies = @()
        }}
    }}
    HelpInfoURI = ''
    DefaultCommandPrefix = ''
}}
";

    this.Log.LogMessage(MessageImportance.Low, manifestContent);

    this.ManifestContent = new TaskItem(manifestContent);

    return true;
  }

  private static (string Prefix, string Suffix) Split(string version, bool includeSeparator)
  {
    int separatorIndex = version.IndexOf('-');

    if (separatorIndex == -1)
    {
      return (version, null);
    }

    string prefix = version.Substring(0, separatorIndex);
    string suffix = version.Substring(includeSeparator ? separatorIndex : separatorIndex + 1, version.Length - separatorIndex);
    return (prefix, suffix);
  }
}
