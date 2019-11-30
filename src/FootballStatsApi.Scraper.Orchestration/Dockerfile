FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./src/FootballStatsApi.Domain/FootballStatsApi.Domain.csproj ./FootballStatsApi.Domain/
COPY ./src/FootballStatsApi.Scraper.Orchestration/FootballStatsApi.Scraper.Orchestration.csproj ./FootballStatsApi.Scraper.Orchestration/
COPY ./src/FootballStatsApi.Scraper.Shared/FootballStatsApi.Scraper.Shared.csproj ./FootballStatsApi.Scraper.Shared/

RUN dotnet restore ./FootballStatsApi.Scraper.Orchestration/FootballStatsApi.Scraper.Orchestration.csproj

COPY ./src/FootballStatsApi.Domain ./FootballStatsApi.Domain
COPY ./src/FootballStatsApi.Scraper.Orchestration ./FootballStatsApi.Scraper.Orchestration
COPY ./src/FootballStatsApi.Scraper.Shared ./FootballStatsApi.Scraper.Shared

RUN dotnet publish FootballStatsApi.Scraper.Orchestration/FootballStatsApi.Scraper.Orchestration.csproj -c Release -o /publish --no-restore

#Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/runtime:3.0
WORKDIR /public
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "FootballStatsApi.Scraper.Orchestration.dll"]