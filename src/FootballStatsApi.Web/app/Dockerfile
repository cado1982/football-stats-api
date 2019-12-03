FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./src/FootballStatsApi.Domain/FootballStatsApi.Domain.csproj ./FootballStatsApi.Domain/
COPY ./src/FootballStatsApi.Web/app/FootballStatsApi.Web.csproj ./FootballStatsApi.Web/

RUN dotnet restore ./FootballStatsApi.Web/FootballStatsApi.Web.csproj

COPY ./src/FootballStatsApi.Domain ./FootballStatsApi.Domain
COPY ./src/FootballStatsApi.Web/app ./FootballStatsApi.Web

RUN dotnet publish FootballStatsApi.Web/FootballStatsApi.Web.csproj -c Release -o /publish --no-restore

#Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /public
COPY --from=build /publish .
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "FootballStatsApi.Web.dll"]