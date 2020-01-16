FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# Setup NodeJs
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
RUN npm -v
# End setup

COPY ./FootballStatsApi.Models/FootballStatsApi.Models.csproj ./FootballStatsApi.Models/
COPY ./FootballStatsApi.Domain/FootballStatsApi.Domain.csproj ./FootballStatsApi.Domain/
COPY ./FootballStatsApi.Logic/FootballStatsApi.Logic.csproj ./FootballStatsApi.Logic/
COPY ./FootballStatsApi.Web/FootballStatsApi.Web.csproj ./FootballStatsApi.Web/

RUN dotnet restore ./FootballStatsApi.Web/FootballStatsApi.Web.csproj

WORKDIR /app/FootballStatsApi.Web
COPY ./FootballStatsApi.Web/package.json .

RUN npm install

COPY ./FootballStatsApi.Web/Styles Styles/
COPY ./FootballStatsApi.Web/gulpfile.js .
RUN npm run gulp-build

WORKDIR /app
COPY ./FootballStatsApi.Models ./FootballStatsApi.Models
COPY ./FootballStatsApi.Domain ./FootballStatsApi.Domain
COPY ./FootballStatsApi.Logic ./FootballStatsApi.Logic
COPY ./FootballStatsApi.Web ./FootballStatsApi.Web

RUN dotnet publish FootballStatsApi.Web/FootballStatsApi.Web.csproj -c Release -o /publish --no-restore

#Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /public
COPY --from=build /publish .
COPY ./certs/FootballStatsApiCert.p12 /etc/ssl/certs/
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "FootballStatsApi.Web.dll"]