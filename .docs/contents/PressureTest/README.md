# 框架压测报告


## 1、测试工具
使用 `JMeter` 进行压力测试。
测试时间：2020年7月1日 13点14分。  
服务器报告:   
<img src="https://img.neters.club/doc/serverreport.png"  >



## 2、测试准备
因为 `JMeter` 是使用 `JAVA` 写的，所以使用 `JMeter` 之前，先安装 `JAVA` 环境。   
安装好后，在 `bin` 文件夹下，点击 `jmeter.bat` 启动程序。  
启动之后会有两个窗口，一个cmd窗口，一个JMeter的 GUI。前面不要忽略CMD窗口的提示信息，不要关闭它。  

## 3、配置数据
本地发布后的 `windows` 环境，直接用 `kestrel` 启动。  
线程数：100  
循环数：10000   
HTTP默认值：协议：`http`；服务器或IP：`localhost`；端口号：`8081`；   
HTTP请求：方法：GET；路径：`/api/blog/ApacheTestUpdate`  
HTTP信息请求管理器：无  
响应断言：无   
 
<img src="https://img.neters.club/doc/ycconfig.png"  >

## 项目配置
目前采用 `Blog.Core` 默认的配置，  
开启了内存 `AOP` 和日志 `AOP`，  
其他的都是默认的，然后也把任务调度也关闭了，  
最后注意要把 `IP限流`给关闭，不然压测没效果，因为限流了：     
<img src="https://img.neters.club/doc/appconfig.png"  >


## 压测结果
1、为了显示正确性，我用动图，来显示日志生成情况，整个阶段无任何异常：  

<img src="https://img.neters.club/doc/ddd.gif"  >


2、内存方面，`100*10000` 的压测过程中，项目保证所占内存在 `160~220m` 之间：

<img src="https://img.neters.club/doc/ycr.png"  >

## 压测配置文件下载
 [配置文件](https://img.neters.club/doc/blog.coretest.jmx)  
 下载后，导入到工具里，可以直接测试，察看结果树。

 ## Docker 镜像
 已经提交到 `docker hub` 自行拉取操作即可：
 ```
 docker pull laozhangisphi/apkimg:latest
 ```