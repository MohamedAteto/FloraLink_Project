@echo off
cd /d "D:\Kiro_Projects"
git add render.yaml FloraLink_Project/Program.cs push_deploy.bat do_push.bat
git commit -m "feat: prepare for Render + Netlify deployment"
git push origin main
echo DONE
pause
