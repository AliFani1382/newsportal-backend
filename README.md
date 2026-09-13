# NewsPortal Backend

A RESTful backend API for a full-stack news platform, built with **ASP.NET Core Web API and .NET 9**.

The project is designed with a layered architecture and focuses on building a maintainable backend with authentication, authorization, database management, news workflows, validation, logging, and practical business features.

## Project Overview

NewsPortal provides the backend services required for a modern news platform, including:

* User authentication and authorization
* News management and publication workflow
* Categories, cities, and tags
* Multiple images for news articles
* Comments and moderation
* Reactions and bookmarks
* Notifications
* Newsletter subscription
* Email verification
* Password reset
* Popular, featured, and related news
* SEO-friendly news slugs
* API validation and error handling
* Logging and API security features

The API is consumed by a React frontend and exposes its endpoints through RESTful APIs.
## 🛠️ Tech Stack

### Backend

* C#
* .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* LINQ
* RESTful APIs
* Dependency Injection
* JWT Authentication & Authorization
* FluentValidation

### Database

* SQL Server
* Entity Framework Core
* EF Core Migrations

### Architecture & Patterns

* Layered Architecture
* Repository Pattern
* Unit of Work
* DTOs
* Service Layer
* SOLID Principles

### Development & Infrastructure

* Swagger / OpenAPI
* Serilog
* Global Exception Handling
* Rate Limiting
* CORS
* Security Headers
* Git & GitHub
* ## 🏗️ Architecture

The backend follows a layered architecture that separates API responsibilities, application logic, domain models, and infrastructure concerns.

```text
NewsPortal
│
├── NewsPortal.API
│   ├── Controllers
│   ├── Middleware
│   └── Configuration
│
├── NewsPortal.Application
│   ├── DTOs
│   ├── Services
│   ├── Interfaces
│   ├── Validators
│   └── Business Logic
│
├── NewsPortal.Domain
│   ├── Entities
│   ├── Enums
│   └── Domain Models
│
└── NewsPortal.Infrastructure
    ├── Persistence
    ├── Repositories
    ├── Services
    └── Data Access
```

### Layer Responsibilities

* **API:** HTTP endpoints, controllers, middleware, authentication configuration, and API configuration.
* **Application:** Business logic, services, DTOs, interfaces, validation, and application workflows.
* **Domain:** Core entities, enums, and domain models.
* **Infrastructure:** Database access, Entity Framework Core, repositories, external services, and infrastructure implementations.

This separation helps keep the codebase maintainable, testable, and easier to extend.
## 🔐 Authentication & Authorization

The API uses JWT-based authentication and role-based authorization to secure protected endpoints.

### Authentication

* User registration and login
* JWT access tokens
* Password hashing
* Email verification
* Password reset flow
* Protected API endpoints

### Authorization

* Role-based access control
* User and Admin roles
* Admin-only operations
* Resource ownership checks for user-owned content
* Authorization using ASP.NET Core `[Authorize]`

Authentication and authorization are integrated into the ASP.NET Core request pipeline to protect application resources and enforce access rules.



