# $msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

.\IncrementVersion-cs.ps1 ..\Leap1
.\IncrementVersion-cs.ps1 ..\Leap2

$slnFilePath = "..\LeapSetup.sln"
& $msbuild $slnFilePath /p:Configuration=Release /t:Clean
& $msbuild $slnFilePath /p:Configuration=Release /t:Rebuild

cd ..\Leap1
.\NuGetPackup.exe
move *.nupkg ..\..\Published -Force
cd ..\Leap2
.\NuGetPackup.exe
move *.nupkg ..\..\Published -Force

explorer ..\..\Published
