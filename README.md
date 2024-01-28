# Planor Backend

<div align="center">
    <img src="https://planorsoft.com/logo.svg" width="200px" height="200px">
</div>

Backend application for planorsoft. Developed using .Net technologies such as Hangfire, Entity Framework Core, Fluent Validation and Automapper. N-Layer architecture and CQRS were used in the architecture of the project. We chose MySQL as our main database because we wanted to benefit from the great support from database providers such as planetscale.

## Development

Planor.WebUI directory is entrypoint for project. Run this project and you can access this project in localhost:5050 

```bash

cd src/Planor.WebUI
dotnet watch run

```

## Local database

This command can be used for running MySQL database with Docker. Connection strings can be found on appsettings.json

```bash

docker compose -f docker-compose.development.yml up -d
docker compose -f docker-compose.development.yml down

```

## Docker

If you do not want to deal with development, you can run the backend project with the database using Docker Compose. You will be able to access the project at localhost:5050.

```bash

docker compose build
docker compose up -d
docker compose down

```