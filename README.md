# Football Stats API

A web scraper to pull stats from understat.com and expose them in a REST api.

## Technology Stack

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

To run locally, execute `docker-compose up --build` from the `src` directory.

Can also be run from inside Visual Studio for debugging individual services.

Check the docker-compose.yml file to discover the port mappings. By default the web app is on localhost:8070 and the api is on localhost:8060. The API has Swagger to browse the endpoints.

You need an API Key to test the API. Register on the website using any random email address. Then check the docker logs to find the link to confirm your email address (make sure to replace &amp; url encoding first). You can also edit the database users table to confirm your email using PGAdmin (the db endpoints are in the docker-compose file). Once you are logged in, you can view your auto-generated api key from the Account section.

