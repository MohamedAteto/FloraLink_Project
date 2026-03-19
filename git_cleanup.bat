@echo off
cd /d "D:\Kiro_Projects"

echo === Removing bin/obj from git tracking ===
git rm -r --cached FloraLink_Project/bin/ 2>nul
git rm -r --cached FloraLink_Project/obj/ 2>nul

echo === Committing cleanup ===
git add .gitignore
git commit -m "chore: remove bin/obj from tracking, fix gitignore"

echo === DONE ===
git log --oneline
