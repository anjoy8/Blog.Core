color B

del  .PublishFiles\*.*   /s /q

dotnet restore

dotnet build

cd Blog.Core

dotnet publish -o ..\Blog.Core\bin\Debug\netcoreapp3.0\

md ..\.PublishFiles

xcopy ..\Blog.Core\bin\Debug\netcoreapp3.0\*.* ..\.PublishFiles\ /s /e 

echo "Successfully!!!! ^ please see the file .PublishFiles"

cmd