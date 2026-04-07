## How to use this file
- Add a short entry after each coding session.
- Capture: what you changed, why, what you learned, and what to improve next.
- Keep notes practical and specific.

## Improvement Notices (from plan review)

### 1) Clarify date/time fields early
- Prefer a single source of truth such as `CreatedAtUtc` and derive date views when possible.
- Keep time handling in UTC and convert only at the edge (UI or API response if required).

### 2) Add API contract rules before implementation grows
- Define validation rules for every request DTO (required fields, max lengths, ranges).
- Define one consistent error response format for all endpoints.
- Add pagination/filtering strategy for list endpoints early.

### 3) Standardize service return model now
- Pick one response approach for services and use it everywhere.
- Recommended: a consistent wrapper (success flag, message, optional data, optional status code).
- This prevents controller rewrites later.

### 4) Plan migration and seed strategy
- Decide how initial `Question` records are created (seed data or startup logic).
- Define migration workflow for local development and future deployment.
- Add unique constraints and indexes where needed (for example: `Question.Code`).

### 5) Include non-functional requirements in the plan
- Testing: service unit tests + API integration tests.
- Configuration: environment-specific settings and secret management.
- Timezone policy: explicit UTC usage and conversion points.

### 6) Lock endpoint style before coding controllers
- Use one API style consistently (REST resource style is recommended).
- Keep naming conventions stable to avoid endpoint churn and client breakage.

### 7) Define relationship delete behavior explicitly
- Decide how related data behaves on delete (`Cascade`, `Restrict`, or soft delete).
- Write this into EF Core configuration so behavior is predictable.

## Session Notes Template

### YYYY-MM-DD - Session Title
- Goal:
- Work done:
- Issues found:
- Decision made:
- What I learned:
- Next step:

## Quick Checklist Before Merging Features
- Domain model is consistent (no duplicate meaning fields).
- DTO validation exists and is tested.
- Service responses follow one standard.
- Database constraints/indexes are present.
- Logging exists for key success/failure paths.
- At least one test covers core behavior.
- Endpoint naming follows project conventions.
