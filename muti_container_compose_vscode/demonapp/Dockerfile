FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["demonapp/demonapp.csproj", "demonapp/"]
RUN dotnet restore "demonapp/demonapp.csproj"
COPY . .
WORKDIR "/src/demonapp"
RUN dotnet publish "demonapp.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "demonapp.dll"]
