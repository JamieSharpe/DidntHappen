@echo off
c:/Users/Jamie/Desktop/DidntHappenTest.exe %1
if %errorlevel% EQU 0 (goto :Success)
if %errorlevel% EQU 1 (goto :NoDocs)
if %errorlevel% EQU 3 (goto :NoPath)
if %errorlevel% EQU 160 (goto :BadArg)
goto :EOF

:Success
echo Success
goto :EOF

:NoPath
echo no path found
goto :PreBuildFailed

:BadArg
echo bad arg
goto :PreBuildFailed

:NoDocs
echo missing documentation.
goto :PreBuildFailed

:PreBuildFailed
echo The prebuild script DidntHappen failed.
exit 1
