git pull;
rm -rf .PublishFiles;
dotnet build;
dotnet publish -o /home/Blog.Core/Blog.Core.Api/bin/Debug/netcoreapp3.1;
cp -r /home/Blog.Core/Blog.Core.Api/bin/Debug/netcoreapp3.1 .PublishFiles;
echo "Successfully!!!! ^ please see the file .PublishFiles";