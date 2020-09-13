#!/bin/bash
docker ps|grep ${1}|while read i;do i; 
echo "容器已启动,详细信息:${i}";
docker stop ${1};
docker rm ${1};
docker rmi ${2};
echo "已关闭容器,${1}" ;
done;