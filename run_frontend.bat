@echo off
cd /d "D:\Kiro_Projects\floralink-dashboard"
echo === Installing packages ===
call "C:\Program Files\nodejs\npm.cmd" install
echo === Starting React dashboard ===
call "C:\Program Files\nodejs\npm.cmd" run dev
