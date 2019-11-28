#Build Stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./FootballStatsApi.sln ./
COPY ./FootballStatsApi/FootballStatsApi.csproj ./FootballStatsApi/
COPY ./FootballStatsApi.Database/FootballStatsApi.Database.csproj ./FootballStatsApi.Database/
COPY ./FootballStatsApi.Scraper.Orchestration/FootballStatsApi.Scraper.Orchestration.csproj ./FootballStatsApi.Scraper.Orchestration/
COPY ./FootballStatsApi.Identity/FootballStatsApi.Identity.csproj ./FootballStatsApi.Identity/
COPY ./FootballStatsApi.Models/FootballStatsApi.Models.csproj ./FootballStatsApi.Models/
COPY ./FootballStatsApi.Scraper.LeagueSummary/FootballStatsApi.Scraper.LeagueSummary.csproj ./FootballStatsApi.Scraper.LeagueSummary/
COPY ./FootballStatsApi.Web/FootballStatsApi.Web.csproj ./FootballStatsApi.Web/
COPY ./FootballStatsApi.Domain/FootballStatsApi.Domain.csproj ./FootballStatsApi.Domain/
COPY ./FootballStatsApi.Scraper.Shared/FootballStatsApi.Scraper.Shared.csproj ./FootballStatsApi.Scraper.Shared/
RUN dotnet restore

COPY . .

RUN dotnet publish FootballStatsApi.Scraper.Orchestration/FootballStatsApi.Scraper.Orchestration.csproj -c Release -o /publish --no-restore

#Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/runtime:3.0
WORKDIR /public
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "FootballStatsApi.Scraper.Orchestration.dll"]