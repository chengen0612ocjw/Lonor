@echo off

if not exist packages (
    md packages
)

for /R "packages" %%s in (*) do (
    del %%s
)

dotnet test test/AspectCore.Lite.Abstractions.Test --configuration release
dotnet test test/AspectCore.Lite.Abstractions.Resolution.Test --configuration release
dotnet test test/AspectCore.Lite.DynamicProxy.Test --configuration release

dotnet pack src/AspectCore.Lite.Abstractions --configuration Release --output packages
dotnet pack src/AspectCore.Lite.Abstractions.Resolution --configuration Release --output packages
dotnet pack src/AspectCore.Lite.DynamicProxy --configuration Release --output packages

set /p key=input key:
set source=http://servicepackages.chinacloudsites.cn/nuget

for /R "packages" %%s in (*symbols.nupkg) do ( 
    call nuget push %%s -s %source%  %key%
)

pause
