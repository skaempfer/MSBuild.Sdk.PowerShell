# PowerShell Sdk Example

## Running the example

1. Run `init-example.ps1`
   - to make the Sdk nuget package available in the example workspace and
   - to initialize a local PowerShellGet repository for this example
2. Open the solution file `PowerShellSdkExample.sln`
3. Build the example project
4. Move the created `.nupkg` file into the PowerShellGet repository folder `powershellget-repository`
5. Run `Install-Module -Name PowerShellSdkExample -Repository ExampleLocal`
6. Run `Get-Greeting` cmdlet from the newly installed module
7. To remove the example repository and example module run `remove-example-artifacts.ps1`
