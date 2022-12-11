$artifactDirectory = $args[0]
$PluginId = "Abc.MoqComplete"
$BuildPropsFilePath = "Abc.MoqComplete\Directory.Build.props"

# Get the version number
if (Test-Path $BuildPropsFilePath) {
	$PluginXml = [xml] (Get-Content $BuildPropsFilePath)
	$Version = $PluginXml.SelectSingleNode(".//Project/PropertyGroup/Version").innerText
}

Write-Host "Generating Rider package"
Write-Host "Staging directory is $artifactDirectory"
Write-Host "Version is $Version"

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

Unzip "$artifactDirectory\$PluginId.Rider.$Version.nupkg" "$artifactDirectory\$PluginId.Rider"

# Move META-INF into its own JAR file
$metaInfSourceLocation = "$artifactDirectory\$PluginId.Rider\META-INF"
$libDirectory = "$artifactDirectory\$PluginId.Rider\lib"
New-Item -Type Directory $libDirectory
Zip "$libDirectory\$PluginId-$Version.jar" $metaInfSourceLocation
Remove-Item -Recurse $metaInfSourceLocation

rm "$artifactDirectory\$PluginId.Rider\_rels" -Force -Recurse
rm "$artifactDirectory\$PluginId.Rider\package" -Force -Recurse
rm "$artifactDirectory\$PluginId.Rider\*.xml"
rm "$artifactDirectory\$PluginId.Rider\*.nuspec"
rm "$artifactDirectory\$PluginId.Rider.$Version.nupkg"
Zip "$artifactDirectory\$PluginId.Rider.$Version.zip" "$artifactDirectory\$PluginId.Rider"
rm "$artifactDirectory\$PluginId.Rider" -Force -Recurse