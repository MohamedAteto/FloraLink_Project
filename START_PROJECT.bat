@echo off
title FloraLink - Start Project
color 0A

echo.
echo  ==========================================
echo   FloraLink Smart Plant Monitoring System
echo  ==========================================
echo.

REM ── Kill any previous instances ──────────────────────────────────────────────
echo [1/3] Stopping any previous instances...
taskkill /F /IM PlantMonitorAPI.exe /T >nul 2>&1
timeout /t 2 /nobreak >nul

REM ── Start Backend ─────────────────────────────────────────────────────────────
echo [2/3] Starting Backend API on http://localhost:5000 ...
start "FloraLink API" cmd /k "cd /d D:\Kiro_Projects\FloraLink_Project && dotnet run --project PlantMonitorAPI.csproj --no-launch-profile"

REM ── Wait for API to boot ──────────────────────────────────────────────────────
echo       Waiting for API to start...
timeout /t 12 /nobreak >nul

REM ── Start Frontend ────────────────────────────────────────────────────────────
echo [3/3] Starting Frontend on http://localhost:5173 ...
start "FloraLink Frontend" cmd /k "cd /d D:\Kiro_Projects\floralink-dashboard && npm run dev"

echo.
echo  ==========================================
echo   Both services are starting!
echo.
echo   Frontend : http://localhost:5173
echo   Backend  : http://localhost:5000
echo   Swagger  : http://localhost:5000/swagger
echo.
echo   Login: demo@floralink.io / demo1234
echo  ==========================================
echo.
pause
