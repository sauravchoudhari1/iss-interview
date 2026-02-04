# Solution Documentation

**Candidate Name:** Saurav Balasaheb Choudhari  
**Completion Date:** 04 feb 2026

---

## Problems Identified

Describe the issues you found in the original implementation. Consider aspects like:

Architecture and design patterns
Code quality and maintainability
Security vulnerabilities
Performance concerns
Testing gaps

The initial project setup had a few critical issues that needed addressing:

- **Depedency Injection Not Used:** depedency injection should be used to loosly couple services and its implementation it also helps in writting unit tests easly.
- **Entity Framework :** Entity Framework should be used to interact with the database instead of raw SQL queries to improve maintainability and reduce boilerplate code.
- **Proper Http Status Codes:** The API endpoints did not return appropriate HTTP status codes for different scenarios (e.g., 201 Created for POST, 404 Not Found for missing resources).
- **Proper Http methods:** The API endpoints did not use the correct HTTP methods for their operations (e.g., GET for retrieval, POST for creation, PUT for updates, DELETE for deletions).
- **Async/Await:** The service and repository methods should be asynchronous to improve scalability and responsiveness.
- **Missing or fragile tests:** Controller tests mocked concrete classes instead of the `ITodoService` interface. Service and repository tests lacked many update/delete/get-by-id and negative-case tests.

---

## Architectural Decisions

Explain the architecture you chose and why. Consider:

To resolve the identified issues, I made several architectural decisions:

- **Layered structure:** Retained the Controller -> Service -> Repository -> DbContext structure to keep the project simple and testable.
- **Dependency injection:** Services and repository are registered via DI and use a scoped `DbContext` per request.
- **Database initialization:** Recommended using EF Core to create the schema from the model by calling `Database.EnsureCreated()` on the registered `TodoDbContext` at startup, instead of creating tables on a separate connection.
- **Testing framework:** Used xUnit for unit tests, along with Moq for mocking dependencies. Added more comprehensive tests for service and repository layers.
---

## Trade-offs

Discuss compromises you made and the reasoning behind them. Consider:

In order to meet the project requirements and deadlines, some trade-offs were made:

- **What was prioritized?:** Fixing the database initialization issue and ensuring the table schema matches the EF Core expectations were top priorities.
- **What was deferred or simplified?:** Some of the more advanced features of EF Core, like Migrations, were not used to keep the initial setup simple.
- **Alternatives considered:** Using raw SQL for schema creation was considered but deemed unnecessary complexity given EF Core's capabilities.
- **Unit tests:** Use Theory, AutoData to auto generate mock data for tests to improve coverage without excessive boilerplate.

---

## How to Run

To run the application and tests, follow these steps:

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version X.Y or later)
- [SQLite](https://www.sqlite.org/download.html) (for database management, optional)

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run --project TodoApi
```

### Test
```bash
dotnet test TodoApi.Tests
```

**Quick runtime fix (apply in `Program.cs`)**

After `var app = builder.Build();` add this block:

```csharp
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
// Create DB schema from EF model if missing
db.Database.EnsureCreated();
```

This ensures EF's model is used to create tables on the same DbContext/connection the app uses. If you prefer to run raw SQL for schema creation, execute those statements on the same `SqliteConnection` instance you passed to `UseSqlite(...)`.

---

## API Documentation

### Endpoints

#### Create TODO
```
Method: POST
URL: /api/todo
Request Body: { "title": "text", "description": "text?", "isCompleted": false }
Response: 201 Created
```

#### Get TODO(s)
```
Method: GET
URL: /api/todo
Request: 
Response: 200 OK
```

```
Method: GET
URL: /api/todo/{id}
Request: 
Response: 200 OK  or 404 Not Found
```

#### Update TODO
```
Method: PUT
URL: /api/todo/{id}
Request Body: UpdateTodoDto
Response: 200 OK  or 404 Not Found
```

#### Delete TODO
```
Method: DELETE
URL: /api/todo/{id}
Request: 
Response: 200 OK  or 404 Not Found
```

**Notes on IDs and models**

The domain layer currently uses `Guid` for IDs. Ensure persistence entities used by EF Core use the same ID type, or introduce a mapping layer between domain DTOs and persistence entities.

---

## Future Improvements

If more time were available, the following improvements would be considered:

- **Controlled schema changes:** Use EF Core Migrations for controlled schema changes instead of `EnsureCreated()`.
- **Integration tests:** Add integration tests using SQLite in-memory with shared connection to better mirror runtime.
- **Global exception handling and logging:** Implement global exception handling, consistent error responses, and logging.
- **Continuous Integration:** Add CI to run tests and static analysis on each change.
- **SQl databse:** Switch from SQLite to a more robust database like SQL Server or PostgreSQL for production use.
