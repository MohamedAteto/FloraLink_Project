@echo off
cd /d "D:\Kiro_Projects"

echo === Staging changes ===
git add FloraLink_Project/Migrations/
git add FloraLink_Project/PlantMonitorAPI.csproj
git add FloraLink_Project/Program.cs

echo === Committing ===
git commit -m "feat: switch to PostgreSQL for deployment"

echo === Pushing ===
git push

echo === DONE ===
git log --oneline -3
