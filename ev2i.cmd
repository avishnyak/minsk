@echo off

REM Vars
set "SLNDIR=%~dp0src"

REM Restore + Build
dotnet build "%SLNDIR%\ev2i" --nologo || exit /b

REM Run
dotnet run -p "%SLNDIR%\ev2i" --no-build
