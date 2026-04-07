# SONYACHNA Backend Working Context

## Product Goal
Single-user backend for lucid-dreaming routine support with 3 modules:
- Survey system for daily variables
- Todo management with scoped planning
- Dream journal

## Domain Model
Enums:
- `TodoStatus`
- `TodoScope`

Entities:
- `SurveySession` (Date, Timestamp, Answers, DreamEntries)
- `Answer` (Value, Remark, QuestionId, SurveySessionId, Question, SurveySession)
- `Question` (Code, Text, Type, Answers)
- `Todo` (Description, Status, Scope, CreationDate, DueDate)
- `DreamEntry` (Content, IsLucid, SurveySessionId, SurveySession)

## Architecture Targets
- Layered structure: Controllers -> Services -> Repositories -> EF Core/SQLite
- Repositories map 1:1 to a dataset/entity and implement a shared `IRepository<T>`
- Services own business logic and return structured operation results (at least success/failure)
- Controllers are thin HTTP adapters with `try/catch`, logging, and proper status codes

## Data + Infra
- EF Core with SQLite
- `database/AppDbContext.cs` with `DbSet` for all entities
- `database/DbFactory.cs` implementing `IDesignTimeDbContextFactory<AppDbContext>`
- SQLite file target: `database/app.db`
- EF migrations under `database/Migrations`

## API Design Rules
- Use versioned endpoints: `/api/v1/...`
- Use plural resource nouns and lowercase/hyphenated paths
- Use HTTP verbs for action semantics (`GET`, `POST`, `PUT`, `DELETE`)

## Planned Endpoint Surface (Normalized)
Survey:
- `POST /api/v1/surveys`
- `GET /api/v1/surveys/exists`

Todos:
- `POST /api/v1/todos`
- `GET /api/v1/todos`
- `GET /api/v1/todos/{id}`
- `PUT /api/v1/todos/{id}`
- `DELETE /api/v1/todos/{id}`
- `POST /api/v1/todos/{id}/complete`
- `POST /api/v1/todos/{id}/fail`

Dream Entries:
- `POST /api/v1/dream-entries`
- `GET /api/v1/dream-entries`
- `GET /api/v1/dream-entries/{id}`
- `PUT /api/v1/dream-entries/{id}`
- `DELETE /api/v1/dream-entries/{id}`
- `GET /api/v1/dream-entries/by-date`

## DTO Coverage
Survey DTOs:
- create survey request/response
- check survey existence request/response

Todo DTOs:
- CRUD request/response models
- mark completed request/response
- mark failed request/response

Dream DTOs:
- CRUD request/response models
- retrieve by date request/response

## Logging + Error Handling Conventions
- Controller methods:
	- wrap service calls in `try/catch`
	- log `Information` on success
	- log `Error` with exception on failure
	- return `StatusCode(500, "An error occurred while processing your request.")` on unhandled errors
- Repositories may throw; services are responsible for handling and shaping errors for controller consumption

## Implementation Sequence
1. Create enums and models
2. Add `AppDbContext` and `DbFactory`
3. Implement generic repository interface and concrete repositories
4. Implement services (survey, todo, dream)
5. Define DTOs
6. Build controllers
7. Register everything in `Program.cs`

## Current Policy For This Repo
- Prefer explicit, simple code over abstraction-heavy patterns
- Keep module responsibilities strict
- Preserve one-user assumptions unless requirements change
