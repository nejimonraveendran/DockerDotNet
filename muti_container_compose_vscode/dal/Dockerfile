FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ./dal .
RUN dotnet restore "dal.csproj"

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

RUN chmod +x ./apply-migration.sh

CMD [ "sh", "./apply-migration.sh" ]