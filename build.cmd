@echo off

REM Vars
set "SLNDIR=%~dp0src"

REM Restore + Build
dotnet build "%SLNDIR%\EV2lang.sln" --nologo || exit /b

REM Test
dotnet test "%SLNDIR%\EV2.Tests" --nologo --no-build
