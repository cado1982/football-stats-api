#Build Stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./FootballStatsApi.Database/FootballStatsApi.Database.csproj ./
RUN dotnet restore

COPY ./FootballStatsApi.Database .

RUN dotnet publish FootballStatsApi.Database.csproj -c Release -o /publish --no-restore

#Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/runtime:3.0
WORKDIR /public
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "FootballStatsApi.Database.dll"]