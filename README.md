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

## 🏗️ Architecture

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

## 📰 News Workflow & Publication

NewsPortal implements a publication workflow that allows users to submit news while giving administrators control over the publishing process.

### News Statuses

* **Draft** — News that is still being prepared.
* **PendingReview** — News submitted by a regular user and waiting for administrator review.
* **Published** — News approved and available for public access.

### User Workflow

Regular users can submit news through the API. Submitted news is placed in the `PendingReview` status and can be reviewed by an administrator before publication.

Users can also manage their own content according to the application's authorization and ownership rules.

### Admin Workflow

Administrators can:

* Review submitted news
* Publish approved news
* Manage news content
* Access administrative operations
* Manage users and other protected resources

This workflow separates content submission from publication and provides basic moderation and authorization for the news platform.

## 🗄️ Database & Entity Framework Core

NewsPortal uses **SQL Server** as its relational database and **Entity Framework Core** for data access and database management.

### Database

* SQL Server
* Entity Framework Core
* Code-first approach
* EF Core migrations
* Entity relationships and configurations
* LINQ-based data queries

### Data Access

The Infrastructure layer is responsible for database access and persistence.

Repositories are used to abstract data access operations, while the Unit of Work pattern coordinates database changes across related operations.

Entity configurations are separated from the domain entities where appropriate to keep persistence concerns organized.

### Main Data Areas

The database manages data related to:

* Users and roles
* News articles
* Categories and cities
* Tags
* News images
* Comments
* Reactions
* Bookmarks
* Notifications
* Newsletter subscriptions
* Email verification and password reset workflows

## 📡 API Documentation & Swagger

The backend provides RESTful API endpoints for the NewsPortal frontend and other API clients.

Swagger / OpenAPI is integrated into the project to provide interactive API documentation and make endpoint testing easier during development.

### API Areas

The API includes endpoints for:

* Authentication and user management
* News and news workflows
* Categories, cities, and tags
* Comments and moderation
* Reactions and bookmarks
* Notifications
* Newsletter subscriptions
* Email verification
* Password reset
* Profile management

Swagger provides documentation for available endpoints, request models, response models, and authentication requirements.

The API follows a consistent response structure to make communication between the backend and frontend predictable.

## 🛡️ Error Handling, Validation & Logging

The backend includes centralized mechanisms for validation, error handling, and application logging.

### Validation

* FluentValidation for request validation
* Validation of user input and API requests
* Consistent validation error responses
* Business rule validation in the application layer

### Error Handling

* Global exception handling middleware
* Consistent API error responses
* Centralized handling of unexpected application errors
* Separation of technical errors from user-facing API responses

### Logging

* Serilog for structured application logging
* Logging of important application events and errors
* Centralized logging configuration

These mechanisms improve API reliability, maintainability, and troubleshooting during development and production use.

## 🔒 Security & API Protection

The API includes several security mechanisms to protect endpoints and control access to application resources.

### Authentication & Access Control

* JWT-based authentication
* Role-based authorization
* Protected API endpoints
* User and Admin access levels
* Resource ownership checks

### API Protection

* Rate limiting to help control excessive requests
* CORS configuration for controlled frontend access
* Security headers for improved HTTP security
* Validation of incoming API requests

These security measures are integrated into the ASP.NET Core application pipeline and help provide a safer and more controlled API environment.

## ⚙️ Configuration & Running the Project

### Prerequisites

Make sure the following tools are installed:

* .NET 9 SDK
* SQL Server or SQL Server LocalDB
* Visual Studio 2022 or another compatible .NET development environment

### Configuration

Before running the API, configure the required application settings, including:

* Database connection string
* JWT authentication settings
* CORS configuration
* File upload settings
* Logging configuration

Sensitive configuration values should be provided through local configuration or environment-specific settings and should not be committed to source control.

### Run the Project

Clone the repository and open the solution in Visual Studio.

Restore dependencies:

```bash
dotnet restore
```

Apply the database migrations if required:

```bash
dotnet ef database update
```

Build the solution:

```bash
dotnet build
```

Then run the API using Visual Studio or the .NET CLI.

Once the API is running, Swagger can be used to explore and test the available endpoints.

## 📂 Project Structure & Key Components

The backend is organized into separate projects based on responsibility:

### NewsPortal.API

Contains the HTTP API layer, including:

* Controllers
* Middleware
* API configuration
* Authentication and authorization setup

### NewsPortal.Application

Contains the application and business logic, including:

* Services
* DTOs
* Interfaces
* Validators
* Application workflows

### NewsPortal.Domain

Contains the core domain models, including:

* Entities
* Enums
* Domain models

### NewsPortal.Infrastructure

Contains infrastructure and data access implementations, including:

* Entity Framework Core
* Database context and configurations
* Repositories
* Infrastructure services
* Data persistence

This structure keeps responsibilities separated and makes the project easier to understand and maintain.

## 📌 Key Features

The backend provides the core functionality required by a modern news platform, including:

* JWT authentication and role-based authorization
* User and Admin management
* News creation, editing, review, and publication workflow
* Categories, cities, and tags
* SEO-friendly unique slugs
* Multiple images per news article
* Comments and comment moderation
* Reactions and bookmarks
* Popular, featured, and related news
* Notifications
* Email verification
* Password reset
* Newsletter subscription
* Request validation with FluentValidation
* Global exception handling
* Structured logging with Serilog
* Rate limiting and security headers
* Swagger / OpenAPI documentation

## 📊 Project Status

The backend development is currently complete and provides the main API functionality required by the NewsPortal application.

The project has been developed as a practical full-stack project with a focus on:

* Clean and maintainable backend architecture
* Authentication and authorization
* Database management with Entity Framework Core
* Real-world business workflows
* API validation and error handling
* Security and application logging
* RESTful API design

The backend is designed to work with the NewsPortal React frontend through RESTful APIs.
