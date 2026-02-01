You are a senior .NET architect. Design and implement a Feature-Based architecture solution for ContosoUniversity with these strict rules:

GOALS
- Use Feature-Based architecture (vertical slices by feature).
- UI must be Razor Pages (NOT MVC controllers for UI).
- Also create a separate ASP.NET Core Web API project that will be used by external consumers (and optionally by UI later).
- Create a Domain Class Library named: ContosoUniversity.Domain
- Both projects (Razor Pages UI and Web API) must reference and call ContosoUniversity.Domain.

STRICT CONSTRAINTS (DO NOT VIOLATE)
- Do NOT use CQRS.
- Do NOT use MediatR.
- Do NOT use AutoMapper. Use manual mapping between Entities <-> DTOs.
- Do NOT use FluentValidation. Use manual validation (data annotations and/or handwritten validation methods).

ARCHITECTURE / FOLDERING
1) In ContosoUniversity.Domain:
   - Create a folder named Features.
   - Inside Features, create folders feature-by-feature (vertical slices), e.g.:
     - Features/Students
     - Features/Courses
     - Features/Enrollments
     (Add more if needed.)
   - In EACH feature folder, keep everything for that feature together:
     - Entities/Models (domain models)
     - DTOs (Request/Response DTOs)
     - Services (feature service interfaces + implementations)
     - Validators (manual validation helpers, if needed)
   - Keep DTOs in the same feature folder as the feature service and models (no global DTO folder).

2) Dependency rule:
   - Razor Pages project calls Feature Services from ContosoUniversity.Domain directly.
   - Web API project calls Feature Services from ContosoUniversity.Domain directly.
   - Razor Pages MUST NOT call API endpoints for its core operations (call the domain services directly).

IMPLEMENTATION DETAILS
- Use EF Core with SQL Server (or SQLite if you must for simplicity, but pick one and be consistent).
- Put DbContext inside ContosoUniversity.Domain (or in a dedicated Infrastructure folder inside Domain if you prefer), and register it from the host projects.
- Each Feature Service should encapsulate the feature’s business logic + data access (simple and pragmatic, not over-engineered).
- Manual mapping:
  - Provide explicit mapping functions in each feature folder (e.g., StudentMappings).
  - Show mapping from Entity -> DTO and DTO -> Entity where needed.
- Manual validation:
  - Use data annotations on DTOs where appropriate.
  - Add additional custom validation methods inside the feature service or a small validator class in the feature folder.
  - Razor Pages should check ModelState and also handle service-level validation errors gracefully.

OUTPUT FORMAT
- Start with the solution tree.
- Then provide code sections per file with clear file paths.
- Ensure the code compiles and runs.
- Include brief notes on how to run migrations and start both projects.

REMINDERS
- Feature-Based folders only (vertical slices).
- Razor Pages for UI.
- No CQRS/MediatR.
- No AutoMapper.
- No FluentValidation.
- Manual DTO mapping and manual validation.
