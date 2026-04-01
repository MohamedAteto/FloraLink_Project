@echo off
echo === Pushing root repo ===
cd /d "D:\Kiro_Projects"
git add .
git commit -m "feat: colorful UI, Settings page, AI OpenRouter, plant data seeding"
git push origin main

echo === Pushing floralink-dashboard repo ===
cd /d "D:\Kiro_Projects\floralink-dashboard"
git add .
git commit -m "feat: colorful UI, Settings page, AI lookup, logo update"
git push origin main

echo === Done ===
pause
