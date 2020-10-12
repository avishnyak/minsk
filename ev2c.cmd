@echo off

REM Vars
set "SLNDIR=%~dp0src"

REM Restore + Build
dotnet build "%SLNDIR%\ev2c" --nologo || exit /b

REM Run
dotnet run -p "%SLNDIR%\ev2c" --no-build -- %*
