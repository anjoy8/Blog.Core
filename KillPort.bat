@echo off
for /f "tokens=5" %%i in ('netstat -aon ^| findstr ":8081"') do (
    set n=%%i
)
taskkill /f /pid %n%

cmd