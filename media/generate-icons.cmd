ECHO OFF

SET MSBUILD="C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"

IF NOT EXIST %MSBUILD% SET MSBUILD="C:\Program Files\dotnet\dotnet.exe" msbuild

ECHO Building using %MSBUILD%

%MSBUILD% /property:GenerateFullPaths=true /t:rebuild resources.proj