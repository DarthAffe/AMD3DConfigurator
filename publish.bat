rmdir /s /q "%~dp0publish\"

dotnet restore AMD3DConfigurator.sln
dotnet build AMD3DConfigurator.sln
dotnet publish -c Release --property:PublishDir="%~dp0publish" /p:DebugType=None /p:DebugSymbols=false AMD3DConfigurator.sln

pause