@echo off
cd /d "D:\Kiro_Projects"
git add .gitignore
git add -f FloraLink_Project/Migrations/
git commit -m "chore: include migrations in repo"
git push
echo === Pushed to GitHub ===
