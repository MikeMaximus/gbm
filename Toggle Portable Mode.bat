@echo off

echo Game Backup Monitor - Portable Mode Switcher
echo Current Folder: %cd%
echo.

if EXIST portable.ini (
    del portable.ini > NUL 2>&1
    if EXIST portable.ini goto :failedoff
    goto :successoff
) else (
    copy /y NUL portable.ini > NUL 2>&1
    if ERRORLEVEL 1 goto :failedon
    goto :successon
)

:successon

echo Game Backup Monitor is now in portable mode.
echo You may run this file again to toggle the mode.
goto :finish

:successoff

echo Game Backup Monitor is now in normal mode.
echo You may run this file again to toggle the mode.
goto :finish

:failedon

echo An error occured, you likely don't have the required permissions.
echo %cd% is not a suitable location for portable mode.
goto :finish

:failedoff

echo An error occured, you likely don't have the required permissions.
echo Try manually deleting the portable.ini file from %cd% to disable portable mode.
goto :finish

:finish

echo.
pause