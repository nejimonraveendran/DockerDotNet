# Dockerfile Demo
The docker file in this folder demonstrates most common docker file commands and how to use them.

```dockerfile
#ARG is sometimes useful to use as a variable to be used in the FROM command.  Note it cannot be used as a generic variable beyond that.
ARG ALPINE_VERSION=latest 

#define base image.  If exists locally, it is pulled from there.  If not, it is pulled from DockerHub by default.  
#you can also specify fully qualified domain name of the registry.  Eg: mcr.microsoft.com/dotnet/aspnet:7.0
FROM alpine:${ALPINE_VERSION}

#defines an environment variable inside the container.  The script.sh and testfile.txt are files existing on the host in the current directory.
ENV CURDIR=/home
ENV SCRIPTFILE=script.sh
ENV SOURCEFILE=testfile.txt


#copy a file from host to container.  
#To copy all files from current host dir to the /home dir on the container, use:  COPY . /home 
COPY ${SOURCEFILE} ${CURDIR}/${SOURCEFILE}

COPY ${SCRIPTFILE} ${CURDIR}/${SCRIPTFILE}

#set working directory inside the container
WORKDIR ${CURDIR}

#run a command inside the container during the BUILD (the available commands depends on the underlying image.  In this eg., alpine).
#RUN <command>  or RUN ["executable", "param1", "param2"] 
RUN chmod +x ${SCRIPTFILE}

#CMD is used to execute a command during the START of the container to provide defaults.  Only one CMD is allowed per docker file.
#CMD ["executable", "param1", "param2"]  or  CMD command param1 param2 
CMD ["sh", "./script.sh"]

#alternative to CMD.
#ENTRYPOINT ["executable", "param1", "param2"]  or  ENTRYPOINT command param1 param2 
#ENTRYPOINT ["./script.sh"]

```
