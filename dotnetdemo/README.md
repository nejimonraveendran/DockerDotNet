## Multi-Stage Dockerfile Build Demo
This folder demonstrates a multi-stage Docker build with Dockerfile.  When a Dockerfile has more than one FROM statement, it is called a multi-stage build.  Each FROM statement is a stage in the build process.  In this demo, the first stage starts with the image *dotnet/sdk:6.0*.  We name the stage as "build".  In this stage, we perform some copy and publish operations. Once the publish is complete, we no longer need the source code and the full .NET SDK for final hosting.  So we start a second stage with the base image *dotnet/aspnet:6.0*.  We just copy the publish output from the previous stage ("base") to the current stage and leave behind all the source code and other artifacts.

### Instructions:

Create a .NET MVC app:

```bash
dotnet new mvc --name dotnetdemo --no-https
```

Add Dockerfile with the following contents:

```dockerfile
#STAGE 1: BUILD CODE
#The image dotnet/sdk:6.0 contains all the tools needed to build .NET Core apps.  We name the stage as "build", which we will need later.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

#set current working dir within the container.  Note that if this path is automatically created if not exists.
WORKDIR /tmp/src

#copy all files in the current host path to the working directory (/tmp/src) in the container file system 
COPY . .

#oncce all files are copied to the container file system, run the command "dotnet restore" to restore nuget packages.  
RUN dotnet restore dotnetdemo.csproj

#now, publish the project into the path /tmp/publish in the container file system.  UseAppHost=false means no need to generate a console app for hosting the web server.
RUN dotnet publish dotnetdemo.csproj -c Release -o /tmp/publish /p:UseAppHost=false

#STAGE 2: BUILD THE FINAL CONTAINER
#for deployment, we just need the asp.net core runtime, not the entire SDK.  So, we re-base the container to aspnet:6.0
FROM mcr.microsoft.com/dotnet/aspnet:6.0

#set the working directory to new path.
WORKDIR /app

#copy only the necessary files from the stage 1, leaving behind the source code files, etc.  For copying th publish output from the stage 1, we use --from=build, where "build" is the name of the stage 1
#copy everything from the path "/tmp/publish" in the previous stage, to the folder "/app" in the current stage. 
COPY --from=build /tmp/publish .

#start the container
ENTRYPOINT ["dotnet", "dotnetdemo.dll"]
```

Build the container image:
```bash
docker build -t dotnetdemo:v1 .
```

Finally, run the container:
```bash
docker run -it --rm -p 8081:80 dotnetdemo:v1
```
