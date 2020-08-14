@ECHO OFF
SET Arbor.Build.Log.Level=Verbose
SET Arbor.Build.BuildNumber.UnixEpochSecondsEnabled=true
SET Arbor.Build.Bootstrapper.AllowPrerelease=true
SET Arbor.Build.Build.Bootstrapper.AllowPrerelease=true
SET Arbor.Build.Tools.External.MSpec.Enabled=true
SET Arbor.Build.NuGet.Package.Artifacts.Suffix=
SET Arbor.Build.NuGet.Package.Artifacts.BuildNumber.Enabled=
SET Arbor.Build.NuGetPackageVersion=
SET Arbor.Build.Vcs.Branch.Name.Version.OverrideEnabled=true
SET Arbor.Build.Vcs.Branch.Name=%GITHUB_REF%
SET Arbor.Build.Build.VariableOverrideEnabled=true
SET Arbor.Build.Artifacts.CleanupBeforeBuildEnabled=true
SET Arbor.Build.Build.NetAssembly.Configuration=
SET Arbor.Build.Tools.External.MSBuild.AllowPrerelease.Enabled=false
SET Arbor.Build.Build.WebProjectsBuild.Enabled=false
SET Arbor.Build.MSBuild.NuGetRestore.Enabled=true
SET Arbor.Build.NuGet.ReinstallArborPackageEnabled=true
SET Arbor.Build.NuGet.VersionUpdateEnabled=false
SET Arbor.Build.Artifacts.PdbArtifacts.Enabled=true
SET Arbor.Build.NuGet.Package.CreateNuGetWebPackages.Enabled=false
SET Arbor.Build.Tools.External.Xunit.NetCoreApp.Enabled=false
SET Arbor.Build.Tools.External.Xunit.NetFramework.Enabled=false

SET Arbor.Build.Build.NetAssembly.MetadataEnabled=true
SET Arbor.Build.Build.NetAssembly.Description=Key value configuration
SET Arbor.Build.Build.NetAssembly.Company=Niklas Lundberg
SET Arbor.Build.Build.NetAssembly.Copyright=© Niklas Lundberg 2014-2020
SET Arbor.Build.Build.NetAssembly.Trademark=Arbor.KVConfiguration
SET Arbor.Build.Build.NetAssembly.Product=Arbor.KVConfiguration
SET Arbor.Build.ShowAvailableVariablesEnabled=true
SET Arbor.Build.ShowDefinedVariablesEnabled=true
SET Arbor.Build.Tools.External.MSBuild.Verbosity=minimal
SET Arbor.Build.NuGet.Package.AllowManifestReWriteEnabled=false
SET Arbor.Build.BuilderNumber.UnixEpochSecondsEnabled=true

SET Arbor.Build.Tools.External.MSBuild.CodeAnalysis.Enabled=false
SET Arbor.Build.Build.PublishDotNetExecutableProjects=false

CALL dotnet arbor-build

REM Restore variables to default

SET Arbor.Build.Build.Bootstrapper.AllowPrerelease=
REM SET Arbor.Build.Vcs.Branch.Name=
SET Arbor.Build.Tools.External.MSpec.Enabled=
SET Arbor.Build.NuGet.Package.Artifacts.Suffix=
SET Arbor.Build.NuGet.Package.Artifacts.BuildNumber.Enabled=
SET Arbor.Build.Log.Level=
SET Arbor.Build.NuGetPackageVersion=
SET Arbor.Build.Vcs.Branch.Name.Version.OverrideEnabled=
SET Arbor.Build.VariableOverrideEnabled=
SET Arbor.Build.Artifacts.CleanupBeforeBuildEnabled=
SET Arbor.Build.Build.NetAssembly.Configuration=

EXIT /B %ERRORLEVEL%
