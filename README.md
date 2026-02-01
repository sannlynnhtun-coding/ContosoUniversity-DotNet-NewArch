# Contoso University - New Architecture

This project is a modern refactoring of the classic Contoso University application, demonstrating a **Service-based Vertical Slice Architecture** on ASP.NET Core 10.0 and Razor Pages.

## Project Overview

Contoso University is a sample application used to demonstrate web development patterns and best practices. This version departs from traditional N-tier architectures in favor of vertical slices, where each feature is self-contained. It also transitions from MediatR-based CQRS to a clean **Domain Service Layer** to simplify logic while maintaining high testability.

## Technology Stack

- **Framework**: ASP.NET Core 10.0 (Razor Pages)
- **ORM**: Entity Framework Core 10.0
- **Database**: SQL Server
- **Architecture**: Vertical Slice Architecture with Domain Services
- **Validation**: FluentValidation
- **Testing**: xUnit, Shouldly, Testcontainers (MsSql), Respawn
- **Migration**: EF Core Migrations & Grate (for integration tests)

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (Required for Integration Tests)
- SQL Server (LocalDB or Express for local development)

### Running the Application

1. Clone the repository.
2. Update the connection string in `appsettings.json` if necessary.
3. Run the application:

   ```powershell
   dotnet run --project ContosoUniversity
   ```

## Database Management

This project uses Entity Framework Core for schema management.

### Adding a New Migration

If you modify entities in `ContosoUniversity.Domain`:

```powershell
dotnet ef migrations add <MigrationName> --project ContosoUniversity.Domain --startup-project ContosoUniversity
```

### Updating the Database

```powershell
dotnet ef database update --project ContosoUniversity.Domain --startup-project ContosoUniversity
```

## Running Tests

The project includes a robust integration testing suite in `ContosoUniversity.IntegrationTests` that uses **Testcontainers** to spin up a real SQL Server instance in Docker.

### Run All Tests

```powershell
dotnet test
```

### Run Specific Test Project

```powershell
dotnet test ContosoUniversity.IntegrationTests
```

## Architecture Details

- **ContosoUniversity.Domain**: Contains entities, the `SchoolContext`, DTOs, and Service implementations. This is the heart of the application logic.
- **ContosoUniversity**: The Razor Pages web application. It consumes the domain services.
- **ContosoUniversity.IntegrationTests**: Uses `SliceFixture` to provide an isolated test environment with a fresh database per test class.

## Useful Commands

### Generate SQL Script

```powershell
dotnet ef migrations script --project ContosoUniversity.Domain --startup-project ContosoUniversity
```

### Tailwind CSS Build (If used)

```powershell
npx tailwindcss -i ./wwwroot/css/app.css -o ./wwwroot/css/site.css --minify
```