echo off
echo "Press B to build images, P to push to registry, any other key to cancel"
set /p op= :
if "%op%"=="B" goto build
if "%op%"=="P" goto push
exit

:build
docker rmi laozhangisphi/apkimg
docker build -f "Dockerfile" --force-rm -t laozhangisphi/apkimg .
goto end

:push
docker push laozhangisphi/apkimg
goto end

:end
pause