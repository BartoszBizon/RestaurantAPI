# RestaurantAPI

ASP.NET Core Web API (.NET 10) for managing restaurants, their dishes and users. A learning project — built step by step as an exercise in EF Core, unit/integration testing, Dapper/SQL injection, streaming (`IAsyncEnumerable`) and Clean Architecture.

## Architecture

The solution is split into 5 projects, following Clean Architecture — dependencies only point inward:

```
RestaurantAPI.Presentation → RestaurantAPI.Infrastructure → RestaurantAPI.Application → RestaurantAPI.Domain
RestaurantAPI.Presentation → RestaurantAPI.Application
RestaurantAPI.Tests        → RestaurantAPI.Presentation
```

| Project | Responsibility |
|---|---|
| **RestaurantAPI.Domain** | Domain entities (`Restaurant`, `Dish`, `Address`, `User`, `Role`) and business exceptions. Zero external dependencies. |
| **RestaurantAPI.Application** | Business logic (services), DTOs, validators (FluentValidation), AutoMapper profile, repository/service interfaces. Knows nothing about EF Core or ASP.NET Core. |
| **RestaurantAPI.Infrastructure** | Repository implementations (EF Core + Npgsql), `RestaurantDbContext`, migrations, `RestaurantSeeder`, `UserContextService` (depends on `IHttpContextAccessor`), and `RestaurantDapperRepository` (SQL injection learning exercise). |
| **RestaurantAPI.Presentation** | Controllers, middleware, authorization (ASP.NET `AuthorizationHandler`), `Program.cs` (composition root, DI). |
| **RestaurantAPI.Tests** | Unit tests (xUnit + Moq) for the services. |

Database access always goes through repositories (`IRestaurantRepository`, `IDishRepository`, `IUserRepository`) — services in `Application` don't know about EF Core.

## Tech stack

- **.NET 10** / ASP.NET Core Web API
- **PostgreSQL** (Npgsql.EntityFrameworkCore.PostgreSQL) — main data access via EF Core
- **Dapper** — learning endpoints for practicing SQL parameterization (see below)
- **AutoMapper**, **FluentValidation**
- **JWT Bearer** — authentication and role-based authorization (`Admin`, `Manager`, `User`)
- **NLog** — logging to files (`C:\RestaurantAPILogs\`)
- **xUnit + Moq** — unit tests
- **Swagger / OpenAPI**

## Requirements

- .NET 10 SDK
- PostgreSQL (local or containerized)
- `dotnet-ef` tool (`dotnet tool install --global dotnet-ef`)

## Getting started

1. **Connection string** — in `RestaurantAPI.Presentation\appsettings.Development.json`, set the `RestaurantDbConnecton` key (yes, missing an "i" — a historical typo kept as-is, since `Program.cs` reads it by that exact name):
   ```json
   "ConnectionStrings": {
     "RestaurantDbConnecton": "Host=localhost;Port=5432;Database=restaurantapi;Username=postgres;Password=your_password"
   }
   ```

2. **Migrations** (the `DbContext` lives in `Infrastructure`, the startup project is `Presentation` — both flags are required):
   ```
   dotnet ef database update --project RestaurantAPI.Infrastructure --startup-project RestaurantAPI.Presentation
   ```

3. **Run the app:**
   ```
   dotnet run --project RestaurantAPI.Presentation
   ```
   On startup, `RestaurantSeeder` automatically seeds sample roles and restaurants if the tables are empty.

4. **Swagger:** `http://localhost:5139/swagger` (`http` profile) or `https://localhost:7089/swagger` (`https` profile) — depending on the chosen launch profile (`RestaurantAPI.Presentation\Properties\launchSettings.json`).

## Tests

```
dotnet test
```

## Main endpoints

| Method | Path | Description |
|---|---|---|
| `POST` | `/api/accounts/register` | Register a user |
| `POST` | `/api/accounts/login` | Log in, returns a JWT |
| `GET` | `/api/restaurant` | List restaurants (search, sorting, paging) |
| `GET` | `/api/restaurant/{id}` | Restaurant details |
| `POST` | `/api/restaurant` | Create a restaurant (`Admin`/`Manager` role) |
| `PUT` / `DELETE` | `/api/restaurant/{id}` | Update / delete (resource-based authorization) |
| `GET` | `/api/restaurant/restaurant-stream` | Streaming demo via `IAsyncEnumerable<T>` |
| `GET`/`POST`/`PUT`/`DELETE` | `/api/{restaurantId}/dish[/...]` | CRUD for dishes within a restaurant |

## ⚠️ Educational endpoints (SQL injection)

`RestaurantDapperRepository` and two endpoints in `RestaurantsController` (`/api/restaurant/dapper-search-vulnearable`, `/api/restaurant/dapper-search-safe`) exist **purely as a learning exercise** for practicing SQL query parameterization with Dapper and defending against SQL injection. The `-vulnearable` version builds SQL via string concatenation **intentionally** and must never be deployed to a production or publicly reachable environment.

## Known issues / TODO

- The `AutoMapper` package is pinned to `12.0.1`, which has a known security advisory (`GHSA-rvv3-g6hj-g44x`) — should be updated at some point (also requires bumping `AutoMapper.Extensions.Microsoft.DependencyInjection`; the AutoMapper 13+ API differs slightly in how it's configured).
