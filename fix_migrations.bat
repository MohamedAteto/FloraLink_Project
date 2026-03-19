@echo off
cd /d "D:\Kiro_Projects\FloraLink_Project"

echo === Killing any running API ===
taskkill /F /IM PlantMonitorAPI.exe /T 2>nul
timeout /t 2 /nobreak >nul

echo === Removing all migrations ===
rmdir /s /q "Migrations" 2>nul

echo === Dropping and recreating migration history in DB ===
sqlcmd -S localhost -d FloraLinkDb -E -Q "DROP TABLE IF EXISTS [__EFMigrationsHistory];" 2>nul

echo === Generating fresh migration (SQL Server) ===
dotnet ef migrations add InitialCreate --output-dir Migrations

echo === Running API (will apply migration + seed) ===
echo Done - now run the API manually or use run_backend.bat
echo === DONE ===
