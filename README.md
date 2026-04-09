# WildTracker

WildTracker is a learning project that simulates a real-world web application used to register and review **wildlife observations**.  
The app works on **synthetic (controlled) data** so it can be reliably used for automation, regression testing, and CI/CD practice.

The main goal is not the domain itself, but building a realistic environment to practice:
- designing a clean backend API,
- creating a modern frontend,
- and setting up a test strategy (unit / API / integration / UI / E2E) that stays fast in CI/CD while still catching defects.

---

## What the app does (MVP)

WildTracker lets users:
- manage a list of **species** (CRUD),
- create and edit **observations** (CRUD),
- browse observations with **filters** (e.g., species and date range),
- authenticate and use role-based permissions (**JWT + RBAC**).

---

## Tech Stack

### Backend
- **.NET (C#)** — ASP.NET Core Web API  
- **PostgreSQL**
- **Entity Framework Core** + migrations
- **JWT authentication** + role-based access control (RBAC)
- **Swagger / OpenAPI** for API documentation

### Frontend
- **React** (separate web app communicating with the backend via REST API)

### Testing
- **xUnit** (unit + integration)
- **Playwright (.NET)** (UI/E2E, plus selected API-level checks)
- Containerized database for integration testing (PostgreSQL in Docker / Testcontainers)

### Dev & Runtime
- **Docker Compose** (repeatable local and CI environments)

---

## Why this project exists

WildTracker is designed to be close to what you see in commercial projects:
- separate frontend + backend,
- authentication and permissions,
- database migrations,
- repeatable environment with Docker,
- automated tests executed in CI/CD.

This makes it a good playground for learning automation engineering and building a strong portfolio project.

---

## Setup and running

### Option A — Docker (recommended)

**Prerequisites:** Docker Desktop installed and running.

```bash
# From the WildTracker/ directory (where docker-compose.yml is located):
docker compose up --build
```

This starts three containers:

| Container | URL |
|-----------|-----|
| Frontend (React) | http://localhost:5173 |
| Backend API + Swagger | http://localhost:5000/swagger |
| PostgreSQL | localhost:5432 |

The database schema is created automatically on first startup. Data is persisted in a named Docker volume (`postgres_data`).

To stop everything:
```bash
docker compose down
```

To stop and delete all data (wipe the database):
```bash
docker compose down -v
```

---

### Option B — Local development

**Prerequisites:** .NET 10 SDK, Node.js 22+, PostgreSQL 16.

**1. Start PostgreSQL** and create a database named `wildtracker` with user `wildtracker` / password `wildtracker`, or update `Api/appsettings.json` with your connection string.

**2. Run the backend:**
```bash
cd Api
dotnet run
# API available at http://localhost:5000
# Swagger UI at http://localhost:5000/swagger
```

**3. Run the frontend:**
```bash
cd frontend
npm install
npm run dev
# App available at http://localhost:5173
```

The Vite dev server proxies `/api/*` requests to `http://localhost:5000`, so the frontend talks to the local backend automatically.

---

## How to use the application

### Animals

Navigate to **Animals** (`/animals`) to:
- View all registered animals in a table
- Create a new animal with identifier, name, species, health status, and optional description
- Delete an animal

### Sighting Reports

Navigate to **Reports** (`/reports`) to:
- Browse sighting reports with optional filters by animal and status
- Create a new report (select animal, date/time observed, report type, source, GPS coordinates)
- **Approve** or **Reject** pending reports
- View and add **observation notes** on any report (click the *Notes* button in a row)
- Delete a report
- Paginate through results (10 per page)

### API (Swagger)

The full REST API is documented at `http://localhost:5000/swagger` (or `http://localhost:5000/swagger` via Docker). Endpoints:

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/animals` | List all animals |
| POST | `/api/animals` | Create animal |
| GET | `/api/animals/{id}` | Get animal by ID |
| PUT | `/api/animals/{id}` | Update animal |
| DELETE | `/api/animals/{id}` | Delete animal |
| GET | `/api/reports` | Search reports (query: animalId, status, page, pageSize) |
| POST | `/api/reports` | Create report |
| GET | `/api/reports/{id}` | Get report by ID |
| PUT | `/api/reports/{id}` | Update report |
| DELETE | `/api/reports/{id}` | Delete report |
| POST | `/api/reports/{id}/approve` | Approve report |
| POST | `/api/reports/{id}/reject` | Reject report |
| GET | `/api/notes/report/{reportId}` | Get notes for a report |
| POST | `/api/notes` | Add observation note |

---
