@echo off
cd /d "D:\Kiro_Projects\FloraLink_Project"

echo === Killing any running API process ===
taskkill /F /IM PlantMonitorAPI.exe /T 2>nul
timeout /t 3 /nobreak >nul

echo === Restoring packages ===
dotnet restore

echo === Removing old migrations ===
rmdir /s /q "Migrations" 2>nul

echo === Generating new Postgres migration ===
dotnet ef migrations add InitialCreate --output-dir Migrations

echo === Building ===
dotnet build -v quiet

echo === DONE ===
