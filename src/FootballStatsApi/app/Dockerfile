FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./FootballStatsApi.Domain/FootballStatsApi.Domain.csproj ./FootballStatsApi.Domain/
COPY ./FootballStatsApi.Models/FootballStatsApi.Models.csproj ./FootballStatsApi.Models/
COPY ./FootballStatsApi/app/FootballStatsApi.csproj ./FootballStatsApi/app/

RUN dotnet restore ./FootballStatsApi/app/FootballStatsApi.csproj

COPY ./FootballStatsApi.Domain ./FootballStatsApi.Domain/
COPY ./FootballStatsApi.Models ./FootballStatsApi.Models/
COPY ./FootballStatsApi/app ./FootballStatsApi/app/

RUN dotnet publish FootballStatsApi/app/FootballStatsApi.csproj -c Release -o /publish --no-restore

#Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /public
COPY --from=build /publish .
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "FootballStatsApi.dll"]