:: see build config for both releases in .csproj file

@echo off

dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo .NET SDK not found. Please install it from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('powershell -Command "(Select-Xml -Path '.\ScritchyScratchyCheater.csproj' -XPath '//Version').Node.InnerText"') do set VERSION=%%i
echo Using build configs for Scritchy Scratchy Cheater by corv1njano...
echo Application Version: %VERSION%

if not exist "Publish" mkdir "Publish"

echo.
echo Publishing Release...
dotnet publish -c Release
if %errorlevel% neq 0 (
    echo Release build FAILED!
    pause
    exit /b 1
)
echo Release build successful!

echo.
echo Publishing ReleaseNetIncluded...
dotnet publish -c ReleaseNetIncluded
if %errorlevel% neq 0 (
    echo ReleaseNetIncluded build FAILED!
    pause
    exit /b 1
)
echo ReleaseNetIncluded build successful!

for /f "tokens=*" %%i in ('powershell -Command "(Select-Xml -Path '.\ScritchyScratchyCheater.csproj' -XPath '//Version').Node.InnerText"') do set VERSION=%%i
set VERSION_FILENAME=%VERSION:.=-%

echo.
echo Compressing builds...
powershell -Command "Compress-Archive -Path 'bin\Release\net9.0-windows\win-x64\publish\*' -DestinationPath 'Publish\ssc_win64_v%VERSION_FILENAME%.zip' -Force"
powershell -Command "Compress-Archive -Path 'bin\ReleaseNetIncluded\net9.0-windows\win-x64\publish\*' -DestinationPath 'Publish\net-included_ssc_win64_v%VERSION_FILENAME%.zip' -Force"

echo.
echo Both builds completed successfully!
echo Publish\ssc_win64_v%VERSION_FILENAME%.zip
echo Publish\net-included_ssc_win64_v%VERSION_FILENAME%.zip
pause