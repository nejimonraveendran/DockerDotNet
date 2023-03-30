## Volume mount demo with SQL Server image
This document demonstrates the Docker volume mounts with the example of using the SQL Server 2022 image

Volumes are used to persist data so that the data is not lost after container is stopped. For this, we can mount storage.  

There are 2 types of storage mounts:
- **Volume mounts:** A storage space managed by Docker in its own location (usually at */var/lib/docker/volumes/*).  Volumes can be created explicitly using ```docker volume create``` command or implictly by using ```--mount``` option of the ```docker run``` command. 
  - List all volumes: ``` docker volume ls ```
  - Create volume: ``` docker volume create <volumename> ```
  - Inspect volume: ``` docker volume inspect <volumename> ```
  - Remove volume: ``` docker volume remove <volumename> ```
  - Remove all unused volumes: ``` docker volume prune ```  
- **Bind mounts:** 


The following example shows a Bind mount.  Here we first create a directory structure */var/local/sql/2022* on the host.  Then we set appropriate permissions for that directory.  After that, we run a container instance based on the official SQL Server 2022 image.  Note that we bind-mount the host directories to the container directories using -v option.  

```bash
#create directory structure on host
mkdir -p /var/local/sql/2022    

#set read, write, special execute permission on the directory on host for all users
sudo chmod --recursive a+rwX /var/local/sql/2022    

#run official SQL server image 
docker run -it --rm  -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=Admin@123 -d -p 1433:1433 --name dockersql --hostname dockersql -v /var/local/sql/2022/data:/var/opt/mssql/data -v /var/local/sql/2022/log:/var/opt/mssql/log -v /var/local/sql/2022/secrets:/var/opt/mssql/secrets mcr.microsoft.com/mssql/server:2022-latest

#just a test: this should print "mssql" as the current container OS user.  Note that this is different than the MSSQL user, which is sa
docker exec -t -i dockersql whoami
```


To access the SQL server through [SQL Server Management Studio 19](https://aka.ms/ssmsfullsetup), use the following auth info:
- Server Name: 127.0.0.1
- Authentication: SQL Server Authentication
- Login: sa
- Password: Admin@123
