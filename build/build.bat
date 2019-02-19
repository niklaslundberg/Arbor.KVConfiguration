@ECHO OFF
SET Arbor.X.Build.Bootstrapper.AllowPrerelease=true
SET Arbor.X.Tools.External.MSpec.Enabled=true
SET Arbor.X.NuGet.Package.Artifacts.Suffix=
SET Arbor.X.NuGet.Package.Artifacts.BuildNumber.Enabled=
SET Arbor.X.NuGetPackageVersion=
SET Arbor.X.Vcs.Branch.Name.Version.OverrideEnabled=false
SET Arbor.X.Build.VariableOverrideEnabled=true
SET Arbor.X.Artifacts.CleanupBeforeBuildEnabled=true
SET Arbor.X.Build.NetAssembly.Configuration=
SET Arbor.X.Tools.External.MSBuild.AllowPrerelease.Enabled=false
SET Arbor.X.Build.WebProjectsBuild.Enabled=false
SET Arbor.X.MSBuild.NuGetRestore.Enabled=true
SET Arbor.X.NuGet.ReinstallArborPackageEnabled=true
SET Arbor.X.NuGet.VersionUpdateEnabled=false
SET Arbor.X.Artifacts.PdbArtifacts.Enabled=true
SET Arbor.X.NuGet.Package.CreateNuGetWebPackages.Enabled=false
SET Arbor.X.Tools.External.Xunit.NetCoreApp.Enabled=false
SET Arbor.X.Tools.External.Xunit.NetFramework.Enabled=false

SET Arbor.X.Build.NetAssembly.MetadataEnabled=true
SET Arbor.X.Build.NetAssembly.Description=Key value configuration
SET Arbor.X.Build.NetAssembly.Company=Niklas Lundberg
SET Arbor.X.Build.NetAssembly.Copyright=© Niklas Lundberg 2014-2018
SET Arbor.X.Build.NetAssembly.Trademark=Arbor.KVConfiguration
SET Arbor.X.Build.NetAssembly.Product=Arbor.KVConfiguration
SET Arbor.X.ShowAvailableVariablesEnabled=false
SET Arbor.X.ShowDefinedVariablesEnabled=false
SET Arbor.X.Tools.External.MSBuild.Verbosity=minimal
SET Arbor.X.NuGet.Package.AllowManifestReWriteEnabled=false

SET Arbor.X.Tools.External.MSBuild.CodeAnalysis.Enabled=false

CALL dotnet arbor-build

REM Restore variables to default

SET Arbor.X.Build.Bootstrapper.AllowPrerelease=
REM SET Arbor.X.Vcs.Branch.Name=
SET Arbor.X.Tools.External.MSpec.Enabled=
SET Arbor.X.NuGet.Package.Artifacts.Suffix=
SET Arbor.X.NuGet.Package.Artifacts.BuildNumber.Enabled=
SET Arbor.X.Log.Level=
SET Arbor.X.NuGetPackageVersion=
SET Arbor.X.Vcs.Branch.Name.Version.OverrideEnabled=
SET Arbor.X.VariableOverrideEnabled=
SET Arbor.X.Artifacts.CleanupBeforeBuildEnabled=
SET Arbor.X.Build.NetAssembly.Configuration=

EXIT /B %ERRORLEVEL%
