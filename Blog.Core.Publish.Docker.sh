# 停止容器
docker stop apkcontainer
# 删除容器
docker rm apkcontainer
# 删除镜像
docker rmi laozhangisphi/apkimg
# 切换目录
cd /home/Blog.Core
# 发布项目
./Blog.Core.Publish.Linux.sh
# 进入目录
cd /home/Blog.Core/.PublishFiles
# 编译镜像
docker build -t laozhangisphi/apkimg .
# 生成容器
docker run --name=apkcontainer -v /etc/localtime:/etc/localtime -it -p 8081:8081 laozhangisphi/apkimg
# 启动容器
docker start apkcontainer
