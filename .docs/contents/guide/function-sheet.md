# H  核心功能一览表

## 一、表结构解析

`Blog.Core` 项目共包含四部分的数据库表结构，分别是：用户角色管理部分、接口菜单权限管理部分、博客文章管理部分、以及其他不重要部分。
> 注意：目前不提供与维护数据库数据，直接通过 `SeedData` 生成种子数据；

### 1、用户角色管理部分[必须]
主要是三个表：分别对应用户表(sysUserInfo)、角色表(Role)、用户角色关系表(UserRole)。  

<img src="http://img.neters.club/doc/usermanager.png" alt="usermanager"  >



### 2、接口菜单权限管理部分[必须]

主要是四个表：分别对应接口表(Module)、菜单表(Permission)、接口菜单关系表(ModulePermission)暂时没用到、角色接口菜单关系表(RoleModulePermission)。  

<img src="http://img.neters.club/doc/permissionmanager.png" alt="permissionmanager"  >




### 3、博客文章管理部分[可选]
主要是三个表：分别对应博客表(BlogArticle)、Bug专题表(Topic)、Bug内容表(TopicDetail)。  

<img src="http://img.neters.club/doc/blogmanager.png" alt="blogmanager"  >




### 4、其他不重要部分

主要是三个表：分别对应Job调度表(TasksQz)、密码库表(PasswordLib)、操作日志表(OperateLog)、广告表(Advertisement)、公告表(Guestbook)。  

<img src="http://img.neters.club/doc/othersmanager.png" alt="othersmanager"  >






## 二、日志记录

本框架涵盖了不同领域的日志记录，共五个，分别是：  
  
1、全局异常日志  
    
    开启方式：无需操作。  
    文件路径：web目录下，Log/GlobalExcepLogs_{日期}.log。  
    功能描述：记录项目启动后出现的所有异常日志，不包括中间件中异常。  

  
2、IP 请求日志    
    
    开启方式：无需操作。  
    文件路径：web目录下，Log/RequestIpInfoLog.log。  
    功能描述：记录项目启动后客户端请求的ip和接口信息。   
    举例来说：  
    {"Ip":"xxx.xx.xx.x","Url":"/api/values","Datetime":"2020-01-06 18:02:19","Date":"2020-01-06","Week":"周一"}

  
3、用户API访问日志    
    
    开启方式：appsettings.json -> Middlewar -> RecordAccessLogs 节点为true。  
    文件路径：web目录下，Log/RecordAccessLogs_{日期}.log。  
    功能描述：记录项目启动后客户端所有的API访问日志，包括参数、body以及用户信息。  

     
4、服务层请求响应AOP日志    
    
    开启方式：appsettings.json -> AppSettings -> LogAOP 节点为true。  
    文件路径：web目录下，Log/AOPLog.log。  
    功能描述：记录项目启动请求api后，所有的service层日志，包括方法名、参数、响应结果或用户（非必须）。 

     
5、数据库操作日志    
    
    开启方式：appsettings.json -> AppSettings -> SqlAOP 节点为true。  
    文件路径：web目录下，Log/SqlLog.log。  
    功能描述：记录项目启动请求api并访问service后，所有的db操作日志，包括Sql参数与Sql语句。
    举例来说：  
    --------------------------------
    1/6/2020 6:13:04 PM|
    【SQL参数】：@bID0:1
    【SQL语句】：SELECT `bID`,`bsubmitter`,`btitle`,`bcategory`,`bcontent`,`btraffic`,`bcommentNum`,`bUpdateTime`,`bCreateTime`,`bRemark`,`IsDeleted` FROM `BlogArticle`  WHERE ( `bID` = @bID0 ) 


  ## 三、控制台信息展示

  <img src="https://img.neters.club/doc/2020-05-09_182758.png" alt="配置" width="800" >



  ## 四、Nginx一览表

  

```
#user  nobody;
worker_processes 1;

#error_log  logs/error.log;
#error_log  logs/error.log  notice;
#error_log  logs/error.log  info;

#pid        logs/nginx.pid;
events {
  worker_connections 1024;
}

http {
  include mime.types;
  default_type application/octet-stream;
  server_names_hash_bucket_size 64;

  #log_format  main  '$remote_addr - $remote_user [$time_local] "$request" '
  #                  '$status $body_bytes_sent "$http_referer" '
  #                  '"$http_user_agent" "$http_x_forwarded_for"';

  #access_log  logs/access.log  main;
  sendfile on;
  #tcp_nopush     on;

  #keepalive_timeout  0;
  keepalive_timeout 600;
  proxy_read_timeout 600;
  proxy_send_timeout 600;

  proxy_buffer_size 128k;
  proxy_buffers 32 32k;
  proxy_busy_buffers_size 128k;

  #gzip  on;
 
  ######################################################################
  server {
    listen 80;
    server_name www.neters.club;

    #charset koi8-r;

    #access_log  logs/host.access.log  main;
    location / {
      root C:\code\Code\Neters\home;
      index index.html index.htm;
    }
  }

  server {
    listen 80;
    server_name neters.club;

    #charset koi8-r;

    #access_log  logs/host.access.log  main;
    location / {
      root C:\code\Code\Neters\home;

      index index.html index.htm;
    }
  }

  server {
    listen 80;
    server_name ids.neters.club;
    rewrite ^(.*)$ https://$host$1 permanent;#把http的域名请求转成https，第二种写法在此节的末端

    #charset koi8-r;

    #access_log  logs/host.access.log  main;
    location / {
      #proxy_pass http://localhost:5004;
      root html;
      index index.html index.htm;
    }
  }

  server {
    listen 443 ssl;
    server_name ids.neters.club; #网站域名，和80端口保持一致
    ssl on;
    ssl_certificate 1_ids.neters.club_bundle.crt; #证书公钥
    ssl_certificate_key 2_ids.neters.club.key; #证书私钥
    ssl_session_cache shared:SSL:1m;
    ssl_session_timeout 5m;
    ssl_protocols TLSv1 TLSv1.1 TLSv1.2;
    ssl_ciphers ECDH:AESGCM:HIGH:!RC4:!DH:!MD5:!3DES:!aNULL:!eNULL;
    ssl_prefer_server_ciphers on;

    error_page 497 https://$host$uri?$args;

    location / {
      proxy_pass http://localhost:5004;
      proxy_redirect off;
      proxy_set_header Host $http_host;
      proxy_set_header X-Real-IP $remote_addr;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;

      proxy_set_header Cookie $http_cookie;
      #proxy_cookie_path
      chunked_transfer_encoding off;
    }
  }

  server {
    listen 80;
    server_name apk.neters.club;

    #charset koi8-r;

    #access_log  logs/host.access.log  main;
    location / {
      root html;
      proxy_pass http://localhost:8081;
      proxy_http_version 1.1;
      proxy_set_header Upgrade $http_upgrade;
      proxy_set_header Connection keep-alive;
      proxy_set_header Host $host;
      proxy_set_header X-Real-IP $remote_addr;
      proxy_cache_bypass $http_upgrade;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;

      index index.html index.htm;
    }

    location /.doc/ {
      proxy_pass http://docs.neters.club/;
    }
  }

  server {
    listen 80;
    server_name docs.neters.club;

    location / {
      root C:\code\Code\Blog.Core\.docs\contents\.vuepress\dist;
      index index.html index.htm;
    }
  }

  server {
    listen 80;
    server_name vueadmin.neters.club;

    location / {
      try_files $uri $uri/ /index.html;
      root C:\code\Code\Blog.Admin\distis;
      #proxy_pass   http://localhost:2364;
      index index.html index.htm;
    }

    location /api/ {
      rewrite ^.+apb/?(.*)$ /$1 break;
      include uwsgi_params;
      proxy_pass http://localhost:8081;
      proxy_http_version 1.1;
      proxy_set_header Upgrade $http_upgrade;
      #proxy_set_header Connection "upgrade";
      #proxy_set_header Host $host;
      proxy_set_header X-Real-IP $remote_addr;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    }

    location /api2/ {
      rewrite ^.+apb/?(.*)$ /$1 break;
      include uwsgi_params;
      proxy_pass http://localhost:8081;
      proxy_http_version 1.1;
      proxy_set_header Upgrade $http_upgrade;
      proxy_set_header Connection "upgrade";
      proxy_set_header Host $host;
      proxy_set_header X-Real-IP $remote_addr;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    }

    location /images/ {
      include uwsgi_params;
      proxy_pass http://localhost:8081;
      proxy_http_version 1.1;
      proxy_set_header Upgrade $http_upgrade;
      #proxy_set_header Connection "upgrade";
      #proxy_set_header Host $host;
      proxy_set_header X-Real-IP $remote_addr;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    }
    location /.doc/ {
      proxy_pass http://docsadmin.neters.club/;
    }

    error_page 404 /404.html;

    # redirect server error pages to the static page /50x.html
    #
    error_page 500 502 503 504 /50x.html;
    location = /50x.html {
      root html;
    }
  }

  server {
    listen 80;
    server_name docsadmin.neters.club;

    location / {
      root C:\code\Code\Blog.Admin\.doc\contents\.vuepress\dist;
      index index.html index.htm;
    }
  }


  server {
    listen 80;
    server_name ddd.neters.club;
    location / {
      proxy_pass http://localhost:4773;
      index index.php index.html index.htm;
      proxy_set_header Upgrade $http_upgrade;
      proxy_set_header Connection keep-alive;
      proxy_set_header Host $host;
      proxy_cache_bypass $http_upgrade;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
      proxy_set_header X-Forwarded-Proto $scheme;

    }
  }


  server {
    listen 80;
    server_name ask.neters.club;

    #charset koi8-r;

    #access_log  logs/host.access.log  main;
    location / {
      root html;
      proxy_pass http://localhost:5020;
      proxy_http_version 1.1;
      proxy_set_header Upgrade $http_upgrade;
      #proxy_set_header Connection "upgrade";
      #proxy_set_header Host $host;
      proxy_set_header X-Real-IP $remote_addr;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
      index index.html index.htm;
    }
  }


  server {
    listen 80;
    server_name vueblog.neters.club;

    location / {
      try_files $uri $uri/ /index.html;
      root C:\code\Code\Blog.Vue\dist;
      index index.html index.htm;
    }


    location /api {
      rewrite ^.+apb/?(.*)$ /$1 break;
      include uwsgi_params;
      proxy_pass http://localhost:8081;
      proxy_set_header X-Real-IP $remote_addr;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    }


    location /images {
      include uwsgi_params;
      proxy_pass http://localhost:8081;
      proxy_set_header X-Real-IP $remote_addr;
      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    }

    error_page 404 /404.html;

    # redirect server error pages to the static page /50x.html
    #
    error_page 500 502 503 504 /50x.html;
    location = /50x.html {
      root html;
    }
  }

  upstream nodenuxt {
    server 127.0.0.1:3089; # nuxt 项目监听PC端端口
    keepalive 64;
  }
  server {
    listen 80;
    server_name tibug.neters.club;

    location / {
      proxy_http_version 1.1;
      proxy_set_header Upgrade $http_upgrade;
      proxy_set_header Connection "upgrade";
      proxy_set_header Host $host;
      proxy_set_header X-Nginx-Proxy true;
      proxy_cache_bypass $http_upgrade;
      proxy_pass http://nodenuxt;
    }

    error_page 500 502 503 504 /50x.html;
    location = /50x.html {
      root html;
    }
  }

  server {
    listen 80;
    server_name jwt.neters.club;

    location / {
      root C:\code\Code\jwttoken;
      index index.html index.htm;
    }

    error_page 404 /404.html;

    # redirect server error pages to the static page /50x.html
    #
    error_page 500 502 503 504 /50x.html;
    location = /50x.html {
      root html;
    }
  }
}

```
>  这里说明下，我的 `Nginx` 文件中，`Ids4` 项目强制使用 `Https` ，采用的是直接跳转，这也是一个办法，当然还有第二种办法(感谢 `tibos`)： 
```
server {
  listen 80;
  server_name admin.wmowm.com;
  location / {
    proxy_pass http://localhost:9002;
    index index.php index.html index.htm;
       proxy_set_header   Upgrade $http_upgrade;
       proxy_set_header   Connection keep-alive;
       proxy_set_header   Host $host;
       proxy_cache_bypass $http_upgrade;
       proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header   X-Forwarded-Proto $scheme;

  }
}

server {
  listen 443 ssl;#监听443端口（https默认端口）
  server_name admin.wmowm.com; #填写绑定证书的域名
  ssl_certificate /etc/nginx/conf.d/key/admin.wm.crt;#填写你的证书所在的位置
  ssl_certificate_key /etc/nginx/conf.d/key/admin.wm.key;#填写你的key所在的位置
  ssl_session_timeout 5m;
  ssl_protocols TLSv1 TLSv1.1 TLSv1.2; #按照这个协议配置
  ssl_ciphers ECDHE-RSA-AES128-GCM-SHA256:HIGH:!aNULL:!MD5:!RC4:!DHE;#按照这个套件配置
  ssl_prefer_server_ciphers on;
  location / {
    proxy_pass http://localhost:9002;
    index index.php index.html index.htm;
    proxy_set_header   Upgrade $http_upgrade;
       proxy_set_header   Connection keep-alive;
       proxy_set_header   Host $host;
       proxy_cache_bypass $http_upgrade;
       proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header   X-Forwarded-Proto $scheme;
  }

}
```