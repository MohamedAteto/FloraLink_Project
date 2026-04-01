@echo off
cd /d "D:\Kiro_Projects\FloraLink_Project"
dotnet build > D:\Kiro_Projects\build_output.txt 2>&1
type D:\Kiro_Projects\build_output.txt
pause
