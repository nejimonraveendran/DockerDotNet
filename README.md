# Docker .NET Samples
This repo is a collection of artifacts (guides, Dockerfile boilerplate templates, demo code, etc) that can be readily used when containerizing .NET applications using Docker.  The artifacts are organized into folders.  

Note:  Look inside the folders in this repo for examples. So far I have added the following folders:
- **[alpinedemo](https://github.com/nejimonraveendran/DockerFundas/tree/main/alpinedemo):** This demonstrates a Dockerfile with most frequenly used commands.
- **[multistage_dotnetdemo](https://github.com/nejimonraveendran/DockerFundas/tree/main/dotnetdemo):** This demonstrates a multi-stage build with Dockerfile.
- **[volumes_sqlserverdemo](https://github.com/nejimonraveendran/DockerSamples/tree/main/sqlserverdemo):** This demonstrates the use of named volumes and bind mounts with a SQL Server container on Linux.
- **[debug_release_demo](https://github.com/nejimonraveendran/DockerSamples/tree/main/debug_release_demo):** This demonstrates how to use the same Dockerfile in C# app for debugging and production phases, using multi-stage build.
- **[multi_container_compose_demo](https://github.com/nejimonraveendran/DockerSamples/tree/main/composedemo):** This demonstrates how to use Docker Compose for development, debugging, production, etc.  Contains 2 .NET apps and a SQL DB 


### Most used run command:
The most handy command: ```docker run -it --rm --name mywebapp -e UNAME=Nejimon -d -p 8080:80 nginx:latest```.  This command:
- Runs the container in interactive mode
- Removes the container after exit
- Name to the container as 'webapp'
- Sets an environment variable UNAME with value Nejimon
- Runs in demon/detached model
- Runs on port 80 and maps it to the host port 8080.


### Command cheat sheet:
| Command | Purpose | Notes |
| --- | --- | --- |
| ```docker images``` | Lists all images on the local system |   |
| ```docker pull <imagename>``` | pulls an image from a remote repo.  If only image name is specified without fully qualified name of the repo, attempt is made to pull it from the Docker Hub | Eg: ```nginxdemos/hello```  |
| ```docker ps -a``` | Lists all running and exited containers  |  |
| ```docker run -p <hostport>:<containerport> <imagename:tag> \| <imageid>``` | Runs a container and maps to a host port. If image does not exist locally, attempt is made to pull from Docker Hub |    Eg: ```docker run -p 8080:80 nginxdemos/hello:latest``` |
| ``` docker run -d <imagename>:tag or image id``` | Runs the container in demon/detached mode.| Eg: ``` docker run -d -p 8080:80 nginxdemos/hello:latest```  |
| ``` docker run --name <name> <imagename>:tag or image id``` | Runs the container and assign it a name.| Eg: ``` docker run --name myapp -p 8080:80 nginxdemos/hello:latest```  |
| ```docker logs <running container name or id>``` | View container logs (useful in detached mode)  |  |
| ``` docker attach <running container name or id>``` | Attach (as foreground) to a container running in detached mode.|   |
| ```docker stop <container name or id>``` | Stops a running container |  |
| ``` docker run -v <hostdir>:<containerdir> <image name or id>``` | runs with volume mapping.| Eg: ``` docker run -v /opt/hostdir:/var/lib/containerdir -p 8080:80 nginxdemos/hello:latest```  |
| ``` docker run -e <key:value> <imagename>:tag or image id``` | Sets an environment variable and value.| Eg: ``` docker run -e MYNAME=Nejimon -p 8080:80 nginxdemos/hello:latest```  |
| ``` docker run --rm <image name or id>``` | automatically remove container after exit.| Eg: ``` docker run --rm -p 8080:80 nginxdemos/hello:latest```  |
| ``` docker run -it <image name or id>``` | runs in interactive mode.| Eg: ``` docker run -it -p 8080:80 nginxdemos/hello:latest```  |
| ```docker rm <container id or name>``` | Delete a container | Must be stopped first before deletion  | |
| ```docker inspect <container id>``` | Look inside the container  |  |
| ```docker container prune``` | Delete all stopped containers | |
| ```docker rmi <image id>``` | Delete an image from the local system |   |
| ```docker image prune -a``` | Deletes all images from the local system |   |
| ```docker exec <container_name> <command>``` | Executes a command on the running container | Eg: ```docker exec ubuntu_id cat > /etc/hosts```  |


### Essential Dockerfile Explained

```dockerfile
# ARG is sometimes useful to use as a variable to be used in the FROM command.  
# Note ARG defined before FROM cannot be used as a generic variable beyond FROM satement.
ARG ALPINE_VERSION=latest 

# Define base image (aka build stage).  If image exists locally, it is pulled from there.  If not, it is pulled from DockerHub by default.  
# You can also specify fully qualified domain name of the registry.  Eg: mcr.microsoft.com/dotnet/aspnet:7.0
FROM alpine:latest AS base

# Define an ARG variable within the scope of the build stage.  
# Unlike the ARG above, Since this is defined after FROM, it can be used as a generic variable.   
ARG CURDIR=/home

# defines an environment variable inside the container.  Quotation marks will not be included in the value read.
ENV CONNECTION_STRING="Connection String"

# set the working directory inside the container.  If the directory does not exist, it is created
WORKDIR /src

# copy everything in the path from host to the specified path in the container.
# note that the source path is determined relative to where the Docker build command is executed, NOT relative to where the Dockerfile is.
# if the directory structure does not exist in the container, it is automatically created.
COPY ["/host/path/to/", "/container/path/to/"]


#run a command inside the container during the BUILD (the available commands depends on the underlying image.  In this eg., alpine).
RUN dotnet tool install --global dotnet-ef

# entrypoint
#CMD is used to execute a command during the START of the container to provide defaults.  Only one CMD is allowed per docker file.
# it can be executed as shell form (using shell) or directly
# format (exec form): CMD ["command", "param1", "param2"]
# format (shell form): CMD command param1 param2

# trick to keep certain containers (eg. alpine) running.  Use "bash" for Ubuntu. 
CMD ["sh", "tail -F anything"] 

#alternative to CMD.
#exec form: ENTRYPOINT ["executable", "param1", "param2"]  or  ENTRYPOINT command param1 param2 
#shell form: ENTRYPOINT command param1 param2
ENTRYPOINT ["sh", "tail -F anything"] 

```





