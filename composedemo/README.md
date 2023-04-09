## Multi-container setup using Docker Compose with debugging capabilities

This document demonstrates how to run multiple containers using Docker Compose.  In this:
- Run all containers using Docker Compose
- Attach debugger to the running containers and step through the code (Ctrl+Shift+P -> "Docker Compose Up").
- Keep different docker compose files for each environment.
- If desired, use the regular VS Code built-in debugger (F5)

### Prequisites:
Make sure the following VS Code extensions are installed:
- C# extension
- Docker extension
- WSL

If the SQL server container has never been run on the dev system before, do the following:
```bash
#create directory structure on host
mkdir -p /var/local/appdata/myapp/dev/sql/2022/    

#set read, write, special execute permission on the directory on host for all users
sudo chmod --recursive a+rwX /var/local/appdata/myapp/dev/sql/
```

### Project creation:
- Web App: ```dotnet new mvc --name webapp --no-https```
- Demon App: ```dotnet new console --name demonapp```
- DAL: ```dotnet new classlib --name dal```

CD into Web APP and Demon App directories, and add reference to DAL: ```dotnet add reference ../dal/dal.csproj```


### Highlights:
- The *.vscode/lanch.json* contains the launch configurations for local debugging as well as Docker run/debugging.
- The *.vscode/tasks.json* contains additional tasks for build/debug
- **dal**
  - Whenever you add/change DB models, run this command to creae a new DB migration: ```dotnet ef migrations add "new_name"```
  - *apply-migration.sh*: To update the DB with the latet migration, you can do either of the following:
    - ```dotnet ef database update``` or
    - ```.\apply-migration.sh``` in VS Code terminal in the ```dal``` directory. Also, this script is automatically executed when Compose is run.  If run directly from bash, it will use DB connection string defined in the *dbsettings.json* file.  If run from Compose, it will use the connection string environment variable defined as in the compose file.
  - *dbsettings.json*: Stores connection string, which is used when migration is run locally (i.e., not through Compose).
- **webapp**
  - *appsettings.json*: Stores connection string, which is used when when application is run directly from VS Code debugger (F5).  When run from the Compose, connection string environment variable defined in the compose file will be used.
- **demonapp**: Simple Console app demonstrating DB connection.
- **docker-compose-db.yml**:  Run this file by using one of the following methods:
  - Right-click -> "Compose Up"
  - From terminal, ```docker compose -f docker-compose-db.yml up``` to start running the DB locally and apply the latest migration.  To stop, use either right-click -> "Compose Down" or ```docker compose -f docker-compose-db.yml down```
  - Ctrl+Shift+P (command palette) -> choose "Docker Compose Up"
- **docker-compose-dev.yml**:  Run this file to run the whole application locally.  Use the same method as as above to launch.  This will do the following:
    - Create 2 networks.
    - run DB server
    - run the web app and the console app
    - apply latest DB migration
    - start an Linux terminal (helpful for admin tasks)
- **docker-compose-prod.yml**:  Production version of the compose file.


### Running the Project in DEV:
Refer to the docker-compose-dev.yml section above.

