SET Configuration=Release
SET localPath=%~dp0

nuget restore ../MoqComplete.sln
"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe" ../MoqComplete.sln /p:OutputPath=%localPath%bin\ /p:Configuration=%Configuration%
nuget pack MoqComplete.nuspec -Version %1
