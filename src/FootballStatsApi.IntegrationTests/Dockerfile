FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./FootballStatsApi.IntegrationTests/FootballStatsApi.IntegrationTests.csproj ./FootballStatsApi.IntegrationTests/
COPY ./FootballStatsApi.Domain/FootballStatsApi.Domain.csproj ./FootballStatsApi.Domain/

WORKDIR /app/FootballStatsApi.IntegrationTests

RUN dotnet restore

WORKDIR /app

COPY ./FootballStatsApi.IntegrationTests ./FootballStatsApi.IntegrationTests/
COPY ./FootballStatsApi.Domain ./FootballStatsApi.Domain/

WORKDIR /app/FootballStatsApi.IntegrationTests
ENTRYPOINT ["dotnet", "test"]