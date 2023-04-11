## Multi-container setup using Docker Compose with debugging capabilities - in Visual Studio 2022

This document demonstrates how to run multiple containers using Docker Compose.  In this:
- Run all containers using Docker Compose
- Attach debugger to the running containers and step through the code (Ctrl+Shift+P -> "Docker Compose Up").
- Keep different docker compose files for each environment.
- If desired, use the regular VS Code built-in debugger (F5)

### Main changes made to VS solution manually (compared to WSL-based development in VS Code):
- Added and set up each project in Docker compose file manually.
- Removed *docker-compose.override.yml* file.
- Set Docker compose project name manually in the dcproj file ```<DockerComposeProjectName>mylinuxapp</DockerComposeProjectName>```
