#This Dockerfile demonstrates multi-stage build.

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

#set the working directory to new path
WORKDIR /app

#copy only the necessary files from the stage 1, leaving the source code files, etc., behind.  For that, we use --from=build, where "build" is the name of the stage 1
#copy everything from the path "/tmp/publish" in the previous stage, to the folder /app in the current stage. 
COPY --from=build /tmp/publish .

#start the container
ENTRYPOINT ["dotnet", "dotnetdemo.dll"]