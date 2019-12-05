FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./FootballStatsApi.Domain/FootballStatsApi.Domain.csproj ./FootballStatsApi.Domain/
COPY ./FootballStatsApi.Models/FootballStatsApi.Models.csproj ./FootballStatsApi.Models/
COPY ./FootballStatsApi/FootballStatsApi.csproj ./FootballStatsApi/

RUN dotnet restore ./FootballStatsApi/FootballStatsApi.csproj

COPY ./FootballStatsApi.Domain ./FootballStatsApi.Domain/
COPY ./FootballStatsApi.Models ./FootballStatsApi.Models/
COPY ./FootballStatsApi ./FootballStatsApi/

RUN dotnet publish FootballStatsApi/FootballStatsApi.csproj -c Release -o /publish --no-restore

#Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /public
COPY --from=build /publish .
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "FootballStatsApi.dll"]