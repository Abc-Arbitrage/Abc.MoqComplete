SET Configuration=Release
SET localPath=%~dp0

nuget restore ../MoqComplete.sln
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe ../MoqComplete.sln /p:OutputPath=%localPath%\bin\ /p:Configuration=%Configuration%
nuget pack MoqComplete.nuspec -Version %1
