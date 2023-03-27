# Docker Fundamentals
This repo is a collection of the artifacts (guides, Dockerfile boilerplate templates, demo code, etc) that can be readily used when containerizing applications using Docker.  The artifacts are organized into folders.  

Note:  Look inside the folders in this repo for examples. So far I have added the following folders:
- **[alpinedemo](https://github.com/nejimonraveendran/DockerFundas/tree/main/alpinedemo):** This demonstrates a Dockerfile with most frequenly used commands.
- **[dotnetdemo](https://github.com/nejimonraveendran/DockerFundas/tree/main/dotnetdemo):** This demonstrates a multi-stage build with Dockerfile.


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








