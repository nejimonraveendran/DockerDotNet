
#STAGE 1: DEBUG
#create a debug layer. The main purpose of this layer is for debugging. Docker vscode extension will create an image out of this base layer and inject the required debug tools (vsdbg)  
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS debug

#define a variable for debug port
ARG DEBUG_PORT=5000

#set url environment variable so that dotnet will run the application on this port
ENV ASPNETCORE_URLS=http://+:${DEBUG_PORT}

#set working dir.  Also create a non-root user (for better security), add permission to access the /app folder, and set the new user as the currently logged in user 
#we can verify the current user in the container's terminal by issuing the command: whoami
WORKDIR /app
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

#inform docker about the container port we are planning to use. 
#this is informational only.  The actual debug port publishing is done through the port command in tasks.json 
EXPOSE ${DEBUG_PORT}
#--------------------------------------------------------------------------------------------------------

#STAGE 2: RELEASE BUILD
#for deployment, we just need the asp.net core runtime, not the entire SDK.  So, we re-base the container to aspnet:6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS release

#set current working dir within the container.  Note that if this path is automatically created if not exists.
WORKDIR /src

#copy all files in the current host path to the working directory in the container file system 
COPY . .

#oncce all files are copied to the container file system, run the command "dotnet restore" to restore nuget packages.  
RUN dotnet restore "dotnetdemo.csproj"

#now, publish the project into the path /src/publish in the container file system.  UseAppHost=false means no need to generate a console app for hosting the web server.
RUN dotnet publish "dotnetdemo.csproj" -c Release -o /src/publish /p:UseAppHost=false


#STAGE 3: PUBLISH FOR DEPLOYMENT
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

#set the working directory to new path
WORKDIR /app

#copy only the necessary files from the RELEASE stage, leaving the source code files, etc., behind.  For that, we use --from=release, where "release" is the name of the previous stage
#copy everything from the path "/src/publish" in the previous stage, to the current working directory (/app) in the current stage. 
COPY --from=release /src/publish .

#inform docker about the container port we are planning to use. 
EXPOSE 80

#set entry point for the container
ENTRYPOINT ["dotnet", "dotnetdemo.dll"]
