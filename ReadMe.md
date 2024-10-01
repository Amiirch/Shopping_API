# E-Commerce Project

This is a .NET-based e-commerce project that provides a RESTful API for managing an online store. The project includes authentication and authorization, uses PostgreSQL with Entity Framework Core for data persistence, Redis for caching to enhance data retrieval speed, and is containerized using Docker. The project also implements unit tests and uses Swagger for API testing and documentation.

## Table of Contents

1. [About the Project](#about-the-project)
2. [Features](#features)
3. [Technologies](#technologies)
4. [Getting Started](#getting-started)
   - [Prerequisites](#prerequisites)
   - [Installation Without Docker](#installation-without-docker)
   - [Installation With Docker](#installation-with-docker)
5. [Usage](#usage)
6. [Running Tests](#running-tests)

---

## About the Project

This project serves as the back-end for an e-commerce platform, offering the following features:

### Features

- User authentication and authorization (JWT)
- Product catalog management
- Redis caching to improve data access speed
- PostgreSQL for data storage with Entity Framework Core
- Docker for easy deployment and containerization
- Swagger for interactive API documentation and testing
- Unit tests to ensure code quality and reliability

---

## Technologies

- **.NET 8**: The core framework used to build the application.
- **Entity Framework Core**: ORM for database interactions with PostgreSQL.
- **PostgreSQL**: Relational database used for data storage.
- **Redis**: Caching layer to improve response times and reduce database load.
- **Docker**: Containerization for deployment and isolated environments.
- **Swagger**: API documentation and testing tool.
- **JWT**: JSON Web Tokens for secure authentication and authorization.

---

## Getting Started

To set up and run the project locally, follow these instructions.

### Prerequisites

Ensure that you have the following installed:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Redis](https://redis.io/download)

---

### Installation Without Docker

1. Clone the repository:

    ```bash
    git clone https://github.com/Amiirchg/E_commerce.git
    ```

2. Navigate to the project directory:

    ```bash
    cd Shopping_API
    ```

3. Configure the connection string in the `appsettings.json` file to point to your PostgreSQL instance.

4. Restore the dependencies:

    ```bash
    dotnet restore
    ```

5. Set up the PostgreSQL database using Entity Framework Core migrations:

    ```bash
    dotnet ef database update
    ```

6. Run the application:

    ```bash
    dotnet run
    ```

---

### Installation With Docker

1. Set up Docker containers for PostgreSQL and Redis using the `docker-compose.yml` file:

    ```bash
    docker compose up --build
    ```

2. Ensure the correct connection string in the `appsettings.json` file, then run migrations to update the PostgreSQL database:

    ```bash
    dotnet ef database update
    ```

---

## Usage

Once the application is running, you can access the API via Swagger for testing and documentation at:

--- 
### Running Tests

for running Test just do it :
   ```bash
   dotnet test
   ```