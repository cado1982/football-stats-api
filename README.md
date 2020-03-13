# Football Stats API

A web scraper to pull stats from understat.com.

This project uses the following technology stack / libraries

- .NET Core
- PostgreSQL
- DBUp
- PuppeteerSharp
- Docker + Docker Compose
- ASP NET Core
- Nginx
- RabbitMQ
- Headless Chrome

It was hosted on DigitalOcean droplets. The container images were built using gitlab ci and pushed to the gitlab container repository. The droplets fetched the images directly and they were spun up using docker-compose.

## Local Development

To run locally, run `docker-compose up --build` from the `src` directory.

Can also be run from inside Visual Studio for debugging individual services.

