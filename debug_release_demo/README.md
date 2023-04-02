## Support Debug and Release in same Dockerfile with multi-stage builds
This folder demonstrates how to use the same Dockerfile in C# app for debugging and production phases, using multi-stage build.

### Instructions:
For c# debugging and other Docker development-related dev support, make sure the following VS Code extensions (all from Microsoft) are installed:
- Docker extension 
- Dev Containers 
- WSL (for WSL-based development)
- C# extension

Create a .NET MVC app:

```bash
dotnet new mvc --name dotnetdemo --no-https
```

Open command palette (Ctrl+Shift+P), then select the option: ```Docker: Add Docker files to Worksapce```. For the prompts, select: 
- Platform: ASP.NET Core
- OS: Linux
- Port: 5000
- Docker Compose: No

The above action will add 3 files to the project:
- Dockerfile: The main docker file with debug and publish stages 
- .vscode/launch.json: App launch and debug support
- .vscode/tasks.json: Tasks definition for launch.

To debug, click the debug button on the left sidebar of VS Code, select "Docker .NET Launch" option from the drop down.  Now, press F5 to run and debug the app. 


### Customizing the build configuration
The default *launch.json* and *tasks.json* contain both Docker-based debugging and process-based debugging, but we can customize it for only Docker-based debugging to achieve dev-prod parity.  To do that, make the following changes:
- Replace the contents of the auto-generated *Dockerfile* with the following:

```dockerfile
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

```
- Replace the contents of the *launch.json* with the following:
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "Docker .NET Debug",
            "type": "docker",
            "request": "launch",
            "preLaunchTask": "docker-run: debug",  //runs a task defined in tasks.json ("label": "docker-run: debug")
            "netCore": {
                "appProject": "${workspaceFolder}/dotnetdemo.csproj"
            }
        }
    ]
}
```

- Replace the contents of the *tasks.json* with the following
```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/dotnetdemo.csproj",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile"
        },

        {
            "type": "docker-build",
            "label": "docker-build: debug",
            "dependsOn": [
                "build"
            ],
            "dockerBuild": {
                "tag": "dotnetdemo:dev",
                "target": "debug",  //must match the STAGE 1 definition in Dockerfile
                "dockerfile": "${workspaceFolder}/Dockerfile",
                "context": "${workspaceFolder}",
                "pull": true
            },
            "netCore": {
                "appProject": "${workspaceFolder}/dotnetdemo.csproj"
            }
        },

        //this task gets invoked first (from launch.json)
        {
            "type": "docker-run",
            "label": "docker-run: debug",
            "dependsOn": [
                "docker-build: debug" //must complete build task first (i.e., "label": "docker-build: debug").
            ],
            //define static ports to be used. By default, vs code switches to different host port on each run
            "dockerRun": { 
                "containerName": "dotnetdemo-dev",
                "ports": [
                {
                    "containerPort": 5000, //this should match the ASPNETCORE_URLS port definition in Dockerfile
                    "hostPort": 5000,
                    "protocol": "tcp"
                }
            ]},
            "netCore": {
                "appProject": "${workspaceFolder}/dotnetdemo.csproj",
                "enableDebugging": true
            }
        }
    ]
}
```

Now, during the development time, launch debugging by pressing F5.

### Release builds / Deployment
When you are ready to release the application and ship it for deployment, use the following command (e.g., through DevOps automation): 

```bash
#"--target final" builds only the "final" stage as defined in the Dockerfile so that only the minimal required runtimes are shipped in the final container 
docker build -t dotnetdemo:latest --target final .
```

Finally, to run the container:
```bash
docker run -it --rm -p 8080:80 dotnetdemo:latest
```
