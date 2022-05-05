git pull;
find .PublishFiles/ -type f -and ! -path '*/wwwroot/images/*' ! -name 'appsettings.*' |xargs rm -rf
dotnet build;
rm -rf /home/Blog.Core/Blog.Core.Api/bin/Debug/.PublishFiles;
dotnet publish -o /home/Blog.Core/Blog.Core.Api/bin/Debug/.PublishFiles;
# cp -r /home/Blog.Core/Blog.Core.Api/bin/Debug/.PublishFiles ./;
awk 'BEGIN { cmd="cp -ri /home/Blog.Core/Blog.Core.Api/bin/Debug/.PublishFiles ./"; print "n" |cmd; }'
echo "Successfully!!!! ^ please see the file .PublishFiles";