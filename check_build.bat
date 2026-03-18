@echo off
cd /d "D:\Kiro_Projects\FloraLink_Project"
echo === Cleaning ===
dotnet clean -v quiet 2>&1
echo === Building ===
dotnet build -v quiet 2>&1
echo === BUILD DONE ===
