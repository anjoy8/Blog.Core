dotnet new -i Blog.Core.Webapi.Template.1.0.0.nupkg

set /p OP=Please set your project name:

md .1YourProject

cd .1YourProject

dotnet new blogcoretpl -n %OP%

cd ../

dotnet new -u Blog.Core.Webapi.Template

pause