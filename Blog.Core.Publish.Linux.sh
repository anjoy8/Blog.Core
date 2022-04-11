git pull;
find .PublishFiles/ -type f -and ! -path '*/wwwroot/images/*' ! -name 'appsettings.*' |xargs rm -rf
dotnet build;
dotnet publish -o /home/Blog.Core/Blog.Core.Api/bin/Debug/net6.0;
cp -r /home/Blog.Core/Blog.Core.Api/bin/Debug/net6.0 .PublishFiles;
echo "Successfully!!!! ^ please see the file .PublishFiles";