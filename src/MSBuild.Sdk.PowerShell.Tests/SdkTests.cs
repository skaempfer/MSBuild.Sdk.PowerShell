using Microsoft.Build.Utilities.ProjectCreation;

namespace MSBuild.Sdk.PowerShell.Tests;

public abstract class SdkTests : MSBuildTestBase, IDisposable
{
  protected string TestRootPath { get; } = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

  protected string TestProjectName { get; } = "PowerShellTestModule";

  protected string TestProjectPath
  {
    get
    {
      return Path.Combine(this.TestRootPath, $"{this.TestProjectName}.csproj");
    }
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected DirectoryInfo CreateFiles(string directoryName, params string[] files)
  {
    DirectoryInfo directory = new DirectoryInfo(Path.Combine(this.TestRootPath, directoryName));

    foreach (FileInfo file in files.Select(i => new FileInfo(Path.Combine(directory.FullName, i))))
    {
      file.Directory?.Create();

      File.WriteAllBytes(file.FullName, new byte[0]);
    }

    return directory;
  }

  protected virtual void Dispose(bool disposing)
  {
    if (disposing)
    {
      if (Directory.Exists(this.TestRootPath))
      {
        try
        {
          Directory.Delete(this.TestRootPath, recursive: true);
        }
        catch (IOException)
        {
          try
          {
            Thread.Sleep(500);

            Directory.Delete(this.TestRootPath, recursive: true);
          }
          catch (IOException)
          {
            // Ignore failures to temp directory removal to avoid test failure
          }
        }
      }
    }
  }

  protected string GetTempFile(string name)
  {
    if (name == null)
    {
      throw new ArgumentNullException(nameof(name));
    }

    return Path.Combine(this.TestRootPath, name);
  }

  protected string GetTempFileWithExtension(string extension)
  {
    return Path.Combine(this.TestRootPath, $"{Path.GetRandomFileName()}{extension}");
  }
}
