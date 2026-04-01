@echo off
cd /d "D:\Kiro_Projects\FloraLink_Project"
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul
dotnet build > D:\Kiro_Projects\build_output.txt 2>&1
echo Exit code: %errorlevel% >> D:\Kiro_Projects\build_output.txt
