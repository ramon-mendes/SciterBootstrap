@echo off

cd win32-resources
rc /I"C:\PROGRAM FILES (X86)\WINDOWS KITS\8.1\Include\um" /I"C:\PROGRAM FILES (X86)\WINDOWS KITS\8.1\Include\shared" Resources.rc
pause