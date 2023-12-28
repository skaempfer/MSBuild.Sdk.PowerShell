using System.Management.Automation;

namespace PowerShellSdkExample;

[Cmdlet(VerbsCommon.Get, "GreetingCmdlet")]
[OutputType(typeof(string))]
public class GetGreetingCmdlet : PSCmdlet
{
  [Parameter(
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; }

  protected override void ProcessRecord()
  {
    WriteObject($"Hello {this.Name}!");
  }
}
