@echo off

if "%1"=="Debug" exit

:: IN Windows, COMPRESS FILES IN /res FOLDER TO ArchiveResource.cs C# FILE
cd %~dp0
packfolder.exe ../res ../Src/ArchiveResource.cs -csharp -x "*IconBundler*;*sciter.dll;.DS_store"