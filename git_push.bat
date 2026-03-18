@echo off
cd /d "D:\Kiro_Projects"

echo === Removing cached bin/obj/node_modules from index ===
git rm -r --cached FloraLink_Project/bin/ 2>nul
git rm -r --cached FloraLink_Project/obj/ 2>nul
git rm -r --cached FloraLink_Project/Migrations/ 2>nul
git rm -r --cached floralink-dashboard/node_modules/ 2>nul

echo === Staging all files ===
git add .

echo === Committing ===
git commit -m "feat: FloraLink Smart Plant Monitoring System - full implementation"

echo === Done - ready to push ===
git remote -v
echo.
echo Run: git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO.git
echo Then: git push -u origin main
