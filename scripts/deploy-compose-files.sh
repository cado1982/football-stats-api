#!/bin/bash

scp ~/Documents/git/football-stats-api/src/docker-compose.production.footballstatsapi-1.yml chrisoliver@178.62.90.155:~/docker-compose.yml
scp ~/Documents/git/football-stats-api/src/docker-compose.production.fsa-api-1.yml chrisoliver@134.209.191.244:~/docker-compose.yml
scp ~/Documents/git/football-stats-api/src/docker-compose.production.fsa-orchestration.yml chrisoliver@68.183.38.189:~/docker-compose.yml
scp ~/Documents/git/football-stats-api/src/docker-compose.production.fsa-web-1.yml chrisoliver@134.209.191.129:~/docker-compose.yml

# chmod u+x ./scripts/deploy-compose-files.sh
