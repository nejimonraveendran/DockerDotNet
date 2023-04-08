## Volume mount demo with SQL Server image
This document demonstrates the Docker volume mounts with the example of using the SQL Server 2022 image

Volumes are used to persist data so that the data is not lost after container is stopped. For this, we can mount storage.  

There are 2 types of storage mounts:
- **Named Volume mounts:** A storage space managed by Docker in its own location (usually at */var/lib/docker/volumes/*).  Volumes can be created explicitly using ```docker volume create``` command or implictly by using ```--mount``` option of the ```docker run``` command. Volume mounts are preferred option if possible.
  - List all volumes: ``` docker volume ls ```
  - Create volume: ``` docker volume create <volumename> ```
  - Inspect volume: ``` docker volume inspect <volumename> ```
  - Remove volume: ``` docker volume remove <volumename> ```
  - Remove all unused volumes: ``` docker volume prune ```  
- **Bind mounts:** An existing storage space on the host that is mapped to a volume.  The main difference here is that bind mounts are not managed by Docker, so host processes can access and modify these mounts. 

**Named Volume mount example:**
The following example shows a volume mount:
```bash
#create volumes.  This is an itempotent command 
docker volume create sql-data-volume 
docker volume create sql-log-volume
docker volume create sql-secrets-volume

#run official SQL server image: Syntax 1 (using -v option for mounting)  
docker run --user root -it --rm  \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD=Admin@123 \
  -p 1433:1433 \
  --name dockersql \
  --hostname dockersql \
  -v sql-data-volume:/var/opt/mssql/data \
  -v sql-log-volume/var/opt/mssql/log \
  -v sql-secrets-volume:/var/opt/mssql/secrets \
  mcr.microsoft.com/mssql/server:2022-latest

#run official SQL server image: Syntax 2 (using --mount option for mounting)  
docker run --user root -it --rm  \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD=Admin@123 \
  -p 1433:1433 \
  --name dockersql \
  --hostname dockersql \
  --mount type=volume,src=sql-data-volume,target=/var/opt/mssql/data \
  --mount type=volume,src=sql-log-volume,target=/var/opt/mssql/log \
  --mount type=volume,src=sql-secrets-volume,target=/var/opt/mssql/secrets \
  mcr.microsoft.com/mssql/server:2022-latest
```


**Bind mount example:**

The following example shows a Bind mount.  Here we first create a directory structure */var/local/sql/2022* on the host.  Then we set appropriate permissions for that directory.  After that, we run a container instance based on the official SQL Server 2022 image.  Note that we bind-mount the host directories to the container directories using -v option.  

```bash
#create directory structure on host
mkdir -p /var/local/sql/2022    

#set read, write, special execute permission on the directory on host for all users
sudo chmod --recursive a+rwX /var/local/sql/2022    

#run official SQL server image: Syntax 1 (using -v option for mounting)  
docker run -it --rm  \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD=Admin@123 \
  -p 1433:1433 \
  --name dockersql \
  --hostname dockersql \
  -v /var/local/sql/2022/data:/var/opt/mssql/data \
  -v /var/local/sql/2022/log:/var/opt/mssql/log \
  -v /var/local/sql/2022/secrets:/var/opt/mssql/secrets \
  mcr.microsoft.com/mssql/server:2022-latest

#run official SQL server image: Syntax 2 (using --mount option for mounting)  
docker run -it --rm  \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD=Admin@123 \
  -p 1433:1433 \
  --name dockersql \
  --hostname dockersql \
  --mount type=bind,src=/var/local/sql/2022/data,target=/var/opt/mssql/data \
  --mount type=bind,src=/var/local/sql/2022/log,target=/var/opt/mssql/log \
  --mount type=bind,src=/var/local/sql/2022/secrets,target=/var/opt/mssql/secrets \
  mcr.microsoft.com/mssql/server:2022-latest
```

To access the SQL server through [SQL Server Management Studio 19](https://aka.ms/ssmsfullsetup), use the following auth info:
- Server Name: 127.0.0.1
- Authentication: SQL Server Authentication
- Login: sa
- Password: Admin@123
