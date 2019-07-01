Param(
    [string]$Configuration = "Release",
    [Parameter(Mandatory=$true)]
    [string]$Version
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

. ".\settings.ps1"

Invoke-Exe $MSBuildPath "/t:Restore;Rebuild;Pack" "$SolutionPath" "/v:minimal" "/p:Configuration=$Configuration" "/p:PackageOutputPath=$OutputDirectory" "/p:PackageVersion=$Version"
