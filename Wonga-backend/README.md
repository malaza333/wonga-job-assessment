# Wonga Assessment — Identity + Audit Microservices (C# + React-ready + PostgreSQL + Docker)

This project implements a simple but professional authentication flow (Register → Login → Get User Details) using a microservice-friendly design **without over-engineering**.

It includes:

- **IdentityService.Api** — main API for Register/Login/User details (JWT)
- **AuditService.Api** — small microservice used to log audit events (e.g., UserRegistered)
- **PostgreSQL** — persistence layer
- **Docker Compose** — runs all services together
- **Unit tests** + **Integration tests** (lightweight, focused)

---

## Tech Stack

- **Backend:** C# (.NET 9)
- **Database:** PostgreSQL
- **Containers:** Docker + Docker Compose
- **API Docs:** Swagger (OpenAPI)
- **Testing:** xUnit + FluentAssertions + Moq (unit) + WebApplicationFactory (integration)
- **Frontend-ready:** CORS enabled for React dev servers

---

## Solution Structure

WongaAssessment.sln
├─ IdentityService.Domain (Entities)
├─ IdentityService.Application (Business logic + interfaces)
├─ IdentityService.Infrastructure (EF Core + Postgres + implementations)
├─ IdentityService.Api (Controllers + middleware + DI + JWT)
├─ AuditService.Api (Audit controller + EF Core + Postgres)
├─ IdentityService.UnitTests
├─ IdentityService.IntegrationTests
└─ AuditService.UnitTests

## Code

---

## How It Works (High Level)

### IdentityService.Api

Handles:

- `POST /api/auth/register` → creates a new user (hashed password)
- `POST /api/auth/login` → returns JWT token
- `GET /api/users/me` → returns current logged-in user details

### AuditService.Api

Handles:

- `POST /api/audit` → stores audit events
- `GET /api/audit?take=20&userId=...` → returns events (optional filter)

### Microservice Interaction

When a user registers successfully:

- IdentityService logs an audit event by calling AuditService.
- Audit is intentionally small to demonstrate microservice architecture in a simple way.

---

## Prerequisites

Install:

- **.NET SDK 9.0**
- **Docker Desktop** (Windows/Mac) or Docker Engine (Linux)

Verify:

```bash
dotnet --version
docker --version
docker compose version

Running the Application (Docker Compose)
1) Start containers

From the solution root:

docker compose up --build

This starts:

identity_api

audit_api

postgres

2) Open Swagger

Identity API:

http://localhost:5001/swagger

Audit API:

http://localhost:5002/swagger

Postgres:

localhost:5432

Environment Variables / Configuration

The APIs use standard configuration via appsettings.json and environment variables.

In Docker, the compose file sets the connection strings and service URLs.

Example important settings:

ConnectionStrings:DefaultConnection

Jwt:Issuer

Jwt:Audience

Jwt:Key

Audit:BaseUrl

Database & Migrations

In development mode, the Identity API runs migrations automatically on startup:

db.Database.Migrate();

AuditService can do the same (if enabled in its Program.cs).

If you ever reset the DB:

docker compose down -v
docker compose up --build
Running Locally (Without Docker)
IdentityService.Api
dotnet run --project IdentityService.Api
AuditService.Api
dotnet run --project AuditService.Api

You must also run Postgres (via Docker or local install) and update connection strings.

Running Tests

From the solution root:

Run all tests
dotnet test
Run unit tests only
dotnet test IdentityService.UnitTests
dotnet test AuditService.UnitTests
Run integration tests
dotnet test IdentityService.IntegrationTests
Build Script
Windows (PowerShell)

Run:

.\build.ps1

What it does:

Restore packages

Build solution (Release)

Run tests

Optionally build Docker images and start containers

API Usage (Example Calls)
1) Register

POST /api/auth/register

Body:

{
  "firstName": "Thomas",
  "lastName": "Selepe",
  "email": "test@mail.com",
  "password": "Password123!"
}

Expected:

204 NoContent if success

409 Conflict if email already exists

2) Login

POST /api/auth/login

Body:

{
  "email": "test@mail.com",
  "password": "Password123!"
}

Response:

{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
3) Get My Details (Protected)

GET /api/users/me

Header:

Authorization: Bearer <token>

Response:

{
  "firstName": "Thomas",
  "lastName": "Selepe",
  "email": "test@mail.com"
}
Notes / Assumptions

Passwords are stored securely using hashing (BCrypt).

JWT is used for authentication.

Audit logging is “best effort” — IdentityService should still succeed even if AuditService is down.

This is intentionally a simplified microservice setup for assessment purposes.
```

Author

Mautloe (Thomas) Selepe
