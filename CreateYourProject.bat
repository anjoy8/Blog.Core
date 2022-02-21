color 5
echo "if u install template error,pls connect QQ:3143422472"


color 3
dotnet new -i Blog.Core.Webapi.Template::2.6.2

set /p OP=Please set your project name(for example:BlogMicService):

md .1YourProject

cd .1YourProject

dotnet new blogcoretpl -n %OP%

cd ../


echo "Create Successfully!!!! ^ please see the folder .1YourProject"

dotnet new -u Blog.Core.Webapi.Template


echo "Delete Template Successfully"

pause
