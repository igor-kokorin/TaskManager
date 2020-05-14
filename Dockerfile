FROM mcr.microsoft.com/dotnet/core/sdk:2.1.806 AS build
WORKDIR /TaskManager

COPY *.sln .
COPY ./TaskManager.Domain/*.csproj ./TaskManager.Domain/
COPY ./TaskManager.Data/*.csproj ./TaskManager.Data/
COPY ./TaskManager.Web/*.csproj ./TaskManager.Web/
RUN dotnet restore

COPY ./TaskManager.Domain/. ./TaskManager.Domain/
COPY ./TaskManager.Data/. ./TaskManager.Data/
COPY ./TaskManager.Web/. ./TaskManager.Web/
WORKDIR /TaskManager/TaskManager.Web/
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1.18 AS runtime
WORKDIR /TaskManager
COPY --from=build /TaskManager/TaskManager.Web/out ./
COPY ./wait-for .

# Необходимо для wait-for
RUN apt-get update && apt-get install netcat -y