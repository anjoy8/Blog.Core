#这种模式是先dotnet build后，然后再把dll进行构建镜像。
#如果你想把这两步合在一起，可以看.sln根目录下的那个dockerfile。

#FROM swr.cn-south-1.myhuaweicloud.com/mcr/aspnet:5.0-alpine
#FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-buster-slim
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
RUN echo 'Asia/Shanghai' >/etc/timezone

#RUN apk add --no-cache ca-certificates python3 bash openssh git openssl-dev uwsgi uwsgi-python3
#RUN apk add --no-cache --virtual .build-deps python3-dev gcc musl-dev libffi-dev make \
        #&& pip3 install --no-cache-dir --trusted-host mirrors.aliyun.com -i http://mirrors.aliyun.com/pypi/simple/ \
                #pymysql==0.8.1 \
                #Flask==1.0.2 \
                #Flask-RESTful==0.3.6 \
                #Flask-Script==2.0.6 \
                #Flask-SQLAlchemy==2.3.2 \
                #Flask-WTF==0.14.2 \
                #SQLAlchemy==1.2.7 \
                #simplejson==5.06.0 \
                #six==1.11.0 \
                #celery==4.2.1 \
                #xlrd==1.1.0 \
                #xlwt==1.3.0 \
                #msgpack==0.5.0 \
                       #&& apk del .build-deps
#
#RUN git clone https://github.com/Supervisor/supervisor.git \
        #&& cd supervisor \
        #&& python3 setup.py install \
        #&& cd .. \
        #&& rm -rf supervisor \
        #&& cd /etc/ \
        #&& echo_supervisord_conf > supervisord.conf \
        #&& echo '[include]' >> supervisord.conf \
        #&& echo 'files = /code/supervisor/*.ini' >> supervisord.conf \
        #&& supervisord -c /etc/supervisord.conf
WORKDIR /app
COPY . . 
EXPOSE 9291 
ENTRYPOINT ["dotnet", "Blog.Core.Api.dll","-b","0.0.0.0"]