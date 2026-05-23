:: see build config for both releases in .csproj-file

@echo off
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

echo.
echo Both builds completed successfully!
pause
