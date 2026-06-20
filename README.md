# GymFlow API

> Secure Gym Management REST API built with ASP.NET Core, EF Core and SQL Server.

GymFlow API is a production-inspired backend project demonstrating authentication, authorization, auditing, testing, and performance optimization techniques commonly used in modern .NET applications.

---

## System Overview

GymFlow API manages a single-branch gym and supports three user roles.

### Admin

* Manage users
* Manage subscriptions and payments
* Create workout plans and exercises
* Monitor gym statistics through dashboards

### Coach

* View assigned members
* Track attendance records
* Monitor member progress

### Member

* View profile information
* Access active subscription details
* View assigned coach
* Access workout plans
* Review attendance history

Role-specific dashboards provide quick insights into gym activities and membership status.

---

## Technical Highlights

### Security

* JWT Authentication
* BCrypt-hashed Refresh Tokens
* Role-Based Authorization
* Ownership-Based Authorization
* HTTPS Enforcement
* CORS Policy
* Rate Limiting
* Audit Logging
* Structured Error Logging
* Global Exception Handling Middleware

### Architecture

* Layered Architecture
* Feature-Based Organization
* Generic Repository Pattern
* Generic Service Layer
* Dependency Injection
* TPT Inheritance Mapping
* Soft Delete with Global Query Filters
* Helper Classes for Shared Constants
* Extension Methods to reduce boilerplate code
* Clean and well-documented codebase

### Performance

* `AsNoTracking()`
* `IgnoreQueryFilters()`
* Disabled `AutoDetectChanges`
* Optimized LINQ Queries
* DTO Projection
* Load-Then-Update strategy

### Testing

Implemented unit tests using **NUnit**

Covered scenarios:

* Login
* Refresh Token
* Logout
* Change Password

---

## Tech Stack

| Backend              | Database   | Testing | Security |
| -------------------- | ---------- | ------- | -------- |
| ASP.NET Core Web API | SQL Server | NUnit   | JWT      |
| EF Core              | Fluent API | Swagger | BCrypt   |

---

## Running the Project

Clone the repository

```bash
git clone https://github.com/your-username/GymFlow-API.git
```

Configure `appsettings.json`

* SQL Server Connection String
* JWT Secret Key

Execute the provided SQL scripts.

Run the project

```bash
dotnet run
```

Swagger is available in **Development mode**.

---

## Demo Accounts

| Role   | Email                                                   | Password |
| ------ | ------------------------------------------------------- | -------- |
| Admin  | [admin@gym.com](mailto:admin@gym.com)                   | 11112222 |
| Coach  | [youssef.adel@gmail.com](mailto:youssef.adel@gmail.com) | 11112222 |
| Member | [khaled.tarek@gmail.com](mailto:khaled.tarek@gmail.com) | 11112222 |
