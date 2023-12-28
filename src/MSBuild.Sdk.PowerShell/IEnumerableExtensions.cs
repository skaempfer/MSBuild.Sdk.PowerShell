namespace MSBuild.Sdk.PowerShell;

public static class IEnumerableExtensions
{
  public static string ToPowerShellArray(this IEnumerable<string> @this) => $"@({string.Join(",", @this.Select(x => $"'{x}'"))})";
}
