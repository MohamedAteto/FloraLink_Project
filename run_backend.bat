@echo off
cd /d "D:\Kiro_Projects\FloraLink_Project"
echo === Applying migrations + seeding data ===
"%USERPROFILE%\.dotnet\tools\dotnet-ef.exe" database update
echo === Starting FloraLink API ===
dotnet run --project PlantMonitorAPI.csproj --no-launch-profile
