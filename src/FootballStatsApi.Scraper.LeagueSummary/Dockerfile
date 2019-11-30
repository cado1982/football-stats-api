#Build Stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY ./src/FootballStatsApi.Domain/FootballStatsApi.Domain.csproj ./FootballStatsApi.Domain/
COPY ./src/FootballStatsApi.Scraper.LeagueSummary/FootballStatsApi.Scraper.LeagueSummary.csproj ./FootballStatsApi.Scraper.LeagueSummary/
COPY ./src/FootballStatsApi.Scraper.Shared/FootballStatsApi.Scraper.Shared.csproj ./FootballStatsApi.Scraper.Shared/

RUN dotnet restore ./FootballStatsApi.Scraper.LeagueSummary/FootballStatsApi.Scraper.LeagueSummary.csproj

COPY ./src/FootballStatsApi.Domain ./FootballStatsApi.Domain
COPY ./src/FootballStatsApi.Scraper.LeagueSummary ./FootballStatsApi.Scraper.LeagueSummary
COPY ./src/FootballStatsApi.Scraper.Shared ./FootballStatsApi.Scraper.Shared

RUN dotnet publish FootballStatsApi.Scraper.LeagueSummary/FootballStatsApi.Scraper.LeagueSummary.csproj -c Release -o /publish --no-restore

#Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/runtime:3.0
WORKDIR /public

#####################
#PUPPETEER RECIPE
#####################
# Install latest chrome dev package and fonts to support major charsets (Chinese, Japanese, Arabic, Hebrew, Thai and a few others)
# Note: this installs the necessary libs to make the bundled version of Chromium that Puppeteer
# installs, work.
RUN apt-get update && apt-get -f install && apt-get -y install wget gnupg2 apt-utils
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - \
    && sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' \
    && apt-get update \
    && apt-get install -y google-chrome-unstable fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst \
      --no-install-recommends \
    && rm -rf /var/lib/apt/lists/*

# Add user, so we don't need --no-sandbox.
# same layer as npm install to keep re-chowned files from using up several hundred MBs more space    
RUN groupadd -r pptruser && useradd -r -g pptruser -G audio,video pptruser \
    && mkdir -p /home/pptruser/Downloads \
    && chown -R pptruser:pptruser /home/pptruser

# Run everything after as non-privileged user.
USER pptruser
#####################
#END PUPPETEER RECIPE
#####################

ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome-unstable"
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "FootballStatsApi.Scraper.LeagueSummary.dll"]