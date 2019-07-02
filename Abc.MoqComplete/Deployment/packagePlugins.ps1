Param(
    [string]$Configuration = "Release",
    [Parameter(Mandatory=$true)]
    [string]$Version
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

. ".\settings.ps1"

# Adapt plugin.xml
$PluginXmlFile = "$SourceBasePath\$PluginId\Resources\plugin.xml"
if (Test-Path $PluginXmlFile) {
	$PluginXml = [xml] (Get-Content $PluginXmlFile)
	$PluginXml.SelectSingleNode(".//idea-plugin/version").innerText = "$Version"
	$PluginXml.Save($PluginXmlFile)
}

# Create packages
Invoke-Exe $MSBuildPath "/t:Restore;Rebuild;Pack" "$SolutionPath" "/v:minimal" "/p:Configuration=$Configuration" "/p:PackageOutputPath=$OutputDirectory" "/p:PackageVersion=$Version"

# Fix rider package
Add-Type -AssemblyName System.Text.Encoding
Add-Type -AssemblyName System.IO.Compression.FileSystem

function Unzip
{
    param([string]$zipfile, [string]$outpath)
    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

class FixedEncoder : System.Text.UTF8Encoding {
    FixedEncoder() : base($true) { }

    [byte[]] GetBytes([string] $s)
    {
        $s = $s.Replace("\", "/");
        return ([System.Text.UTF8Encoding]$this).GetBytes($s);
    }
}

function Zip
{
    param([string]$zipfile, [string]$outpath)
    [System.IO.Compression.ZipFile]::CreateFromDirectory($outpath, $zipfile, [System.IO.Compression.CompressionLevel]::Optimal, $True, [FixedEncoder]::new())
}

Unzip "$OutputDirectory\$PluginId.Rider.$Version.nupkg" "$OutputDirectory\$PluginId.Rider"
rm "$OutputDirectory\$PluginId.Rider\_rels" -Force -Recurse
rm "$OutputDirectory\$PluginId.Rider\package" -Force -Recurse
rm "$OutputDirectory\$PluginId.Rider\*.xml"
rm "$OutputDirectory\$PluginId.Rider\*.nuspec"
rm "$OutputDirectory\$PluginId.Rider.$Version.nupkg"
Zip "$OutputDirectory\$PluginId.Rider.$Version.zip" "$OutputDirectory\$PluginId.Rider"
rm "$OutputDirectory\$PluginId.Rider" -Force -Recurse