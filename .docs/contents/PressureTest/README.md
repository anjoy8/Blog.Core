# 框架压测报告


## 1、测试工具
使用 `JMeter` 进行压力测试。
测试时间：2021年1月21日 09点38分。  
服务器报告:   
<img src="https://img.neters.club/doc/report.png"  >



## 2、测试准备
因为 `JMeter` 是使用 `JAVA` 写的，所以使用 `JMeter` 之前，先安装 `JAVA` 环境。   
安装好后，在 `bin` 文件夹下，点击 `jmeter.bat` 启动程序。  
启动之后会有两个窗口，一个`cmd`窗口，一个`JMeter`的 `GUI`。前面不要忽略`CMD`窗口的提示信息，不要关闭它。    
注意：使用`API`模式，不要使用`GUI`模式。


## 3、测试配置
本地发布后的 `windows` 环境，直接用 `kestrel` 启动。  
线程数：100  
循环数：1000   
HTTP默认值：协议：`http`；服务器或IP：`localhost`；端口号：`8081`；   
HTTP请求：方法：GET；路径：`/api/blog/ApacheTestUpdate`  
HTTP信息请求管理器：无  
响应断言：无   
 
<img src="https://img.neters.club/doc/config.png"  >

## 项目初始化配置
目前采用 `Blog.Core` 默认的配置，  
只开启了内存 `AOP` ，  
其他的都是默认的，然后也把任务调度也关闭了，  
最后注意要把 `IP限流`给关闭，不然压测没效果，因为限流了：     
<img src="https://img.neters.club/doc/init.png"  >


## 压测过程
### 1、为了显示正确性，我用动图，来显示日志生成情况，整个阶段无任何异常：  

##### 第一阶段
<img src="https://img.neters.club/doc/test01.png"  >  
##### 第二阶段
<img src="https://img.neters.club/doc/test02.png"  >  
##### 第三阶段
<img src="https://img.neters.club/doc/test03.png"  > 
##### 第四阶段（压测后，检测内存是否降低）
<img src="https://img.neters.club/doc/test04.png"  >  


### 2、内存方面，`100*1000` 的压测过程中（写操作），项目保证所占内存在 `350~500m` 之间：

<img src="https://img.neters.club/doc/test04.png"  > 

## 压测配置文件下载
 [配置文件](https://img.neters.club/doc/blogcore_blog_ApacheTestUpdate.jmx)  
 下载后，导入到工具里，可以直接测试，察看结果树。

 ## Docker 镜像
 已经提交到 `docker hub` 自行拉取操作即可：
 ```
 docker pull laozhangisphi/apkimg:latest
 ```
