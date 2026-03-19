@echo off
cd /d "%~dp0"
git add -A
git commit -m "feat: prepare for Render + Netlify deployment"
git push
echo Done.
pause
