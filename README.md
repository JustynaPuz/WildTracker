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
