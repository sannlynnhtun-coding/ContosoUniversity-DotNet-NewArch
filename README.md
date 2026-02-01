![CI](https://github.com/jbogard/ContosoUniversityDotNetCore-Pages/workflows/CI/badge.svg)

# ContosoUniversity on ASP.NET Core 10.0 on .NET 10 and Razor Pages

Contoso University, the way I would write it.

To prepare the database, run the Aspire Migrate command on the database (after starting Aspire).

## Things demonstrated

- CQRS and MediatR
- AutoMapper
- Vertical slice architecture
- Razor Pages
- Fluent Validation
- HtmlTags
- Entity Framework Core


1. Create a New Migration
Use this when you have modified your domain entities (e.g., 
Student
, Course, Instructor) and need to generate a corresponding database migration.

```powershell
dotnet ef migrations add <YourMigrationName> --project ContosoUniversity.Domain --startup-project ContosoUniversity
```
Replace <YourMigrationName> with a descriptive name, like AddStudentEmail or InitialCreate.

2. Update the Database
Use this to apply pending migrations to your database.

powershell
dotnet ef database update --project ContosoUniversity.Domain --startup-project ContosoUniversity
3. (Optional) Generate SQL Script
If you need a SQL script for production deployment:

```powershell
dotnet ef migrations script --project ContosoUniversity.Domain --startup-project ContosoUniversity
```

```bash
sqlcmd -S . -U sa -P sasa@123 -d ContosoUniversity -i "new data.sql"
```