# SkillHub — Workshop & Enrollment Management Platform

## 1. Project Description

**SkillHub** is a training center management platform built in **.NET 10** following **Clean Architecture**, **Domain-Driven Design (DDD)**, and **SOLID Principles**.

The solution consists of a **RESTful API** (`SkillHub.Api`) and an **ASP.NET Core MVC Web Application** (`SkillHub.Web`, Presentation Option A) that enable SkillHub to publish in-person workshops and manage participant enrollments while strictly enforcing domain business rules:
- **Capacity Control:** Prevents exceeding the workshop's maximum capacity (counting only active enrollments).
- **No Duplicate Enrollments:** Prevents a participant from holding two active enrollments in the same workshop.
- **Schedule Conflict Detection:** Prevents enrolling in a workshop whose schedule overlaps with another active enrollment of the same participant.
- **Future Workshops Only:** Prevents creating or enrolling in workshops that have already started, and validates that `EndDate > StartDate`.
- **Cancellation & Audit Rules:** Participants can cancel up to 24 hours before the workshop starts (releasing the spot), whereas Employees can cancel any enrollment before the workshop starts without the 24-hour restriction. Every cancellation records who performed it (`CancelledBy`) and when (`CancelledAtUtc`).
- **Re-enrollment:** Allows participants who previously cancelled an enrollment to re-enroll if spots are available and all rules are met.
- **Bulk Excel Import:** Allows employees to upload `SkillHub_Data.xlsx` (`Enrollments` sheet) to process enrollments row by row, persisting valid rows and returning detailed rejection reasons with row numbers.
- **PDF Enrollment Certificate:** Generates an academic enrollment certificate in PDF for active enrollments behind an application-layer abstraction.

---

## 2. Technologies Used

- **Framework:** .NET 10 (`ASP.NET Core Web API` & `ASP.NET Core MVC`)
- **Architecture & Patterns:** Clean Architecture (`Domain`, `Application`, `Infrastructure`, `Api`, `Web`), Rich Domain Model, Value Objects, Repository Pattern, Dependency Injection, DTOs
- **Database & ORM:** **PostgreSQL 15+** & **Entity Framework Core 10** (Code-First & Migrations via `Npgsql.EntityFrameworkCore.PostgreSQL`)
- **Authentication & Authorization:** **ASP.NET Core Identity**, **JWT Bearer Authentication** (for the REST API), Cookie Authentication (for the MVC Web App), and Role-Based Authorization (`Employee`, `Participant`)
- **API Documentation:** OpenAPI + **Scalar** (`Scalar.AspNetCore`)
- **PDF Generation:** **QuestPDF** (implemented in Infrastructure behind `ICertificatePdfGenerator`)
- **Excel Processing:** **EPPlus** (implemented in Infrastructure behind `IExcelImportParser`)
- **Frontend Styling:** **Tailwind CSS** + Semantic HTML5

---

## 3. Project Architecture

```text
SkillHub/
├── README.md
├── SkillHub.sln
├── SkillHub_Database.sql
└── src/
    ├── SkillHub.Domain/           # Enterprise business rules, Entities, ValueObjects, Enums, Errors, Exceptions
    ├── SkillHub.Application/      # Use cases, Request/Response DTOs, Interfaces, Application Services, DI
    ├── SkillHub.Infrastructure/   # EF Core ApplicationDbContext, Fluent API Configurations, Migrations, Repositories, Identity, JWT, PDF, Excel
    ├── SkillHub.Api/              # REST Controllers (/api/v1/*), ExceptionHandlingMiddleware, Scalar UI
    └── SkillHub.Web/              # ASP.NET Core MVC Participant Portal (consumes Application Services directly)
```

---

## 4. Setup and Execution Instructions (Including Database)

### Prerequisites
- **.NET 10 SDK**
- **PostgreSQL 15+** running on `localhost:5432`

### Step 1: Clone the Repository
```bash
git clone https://github.com/juanjo2409/SkillHub.git
cd SkillHub
```

### Step 2: Configure the Database Connection
Verify the PostgreSQL connection string in both `src/SkillHub.Api/appsettings.json` and `src/SkillHub.Web/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=SkillHub;Username=postgres;Password=Zxc.123*"
}
```

### Step 3: Create and Seed the Database (Code-First Migrations)
Run the following command from the root directory to apply EF Core migrations and automatically seed the initial **`Instructors` (6)**, **`Participants` (30)**, and **`Workshops` (15)** sheets from `SkillHub_Data.xlsx`:
```bash
dotnet ef database update --project src/SkillHub.Infrastructure --startup-project src/SkillHub.Api
```
> **Note:** The `Enrollments` sheet is intentionally left unseeded at startup as required by the specification so it can be imported via `POST /api/v1/enrollments/import`. Alternatively, you can restore the database directly using the included [`SkillHub_Database.sql`](./SkillHub_Database.sql) script.

### Step 4: Build and Run the REST API (`SkillHub.Api`)
```bash
dotnet build SkillHub.sln
dotnet run --project src/SkillHub.Api
```
- **API Base URL:** `http://localhost:5275`
- **Scalar Interactive API Documentation:** `http://localhost:5275/scalar/v1`

### Step 5: Run the Web Application (`SkillHub.Web`)
In a separate terminal, run:
```bash
dotnet run --project src/SkillHub.Web
```
- **Web Portal URL:** `http://localhost:5108`

---

## 5. Employee & Participant Credentials

### Employee Accounts (Role: `Employee`)
All 6 instructors from `SkillHub_Data.xlsx` are pre-seeded at startup and can log in from the very first run:

| Full Name | Email | Password | Specialty |
| :--- | :--- | :--- | :--- |
| Andrés Felipe Mejía Arango | `andres.mejia@correo.com` | `Instructor2026*` | Photography (`Fotografía`) |
| Rosa Elena Barrios Polo | `rosa.barrios@correo.com` | `Instructor2026*` | Culinary Arts (`Cocina`) |
| Mauricio Jiménez Cárdenas | `mauricio.jimenez@correo.com` | `Instructor2026*` | Public Speaking (`Oratoria`) |
| Natalia Herrera Castaño | `natalia.herrera@correo.com` | `Instructor2026*` | Graphic Design (`Diseño gráfico`) |
| Hernán Darío Quintero Gil | `hernan.quintero@correo.com` | `Instructor2026*` | Music (`Música`) |
| Paola Andrea Vargas Mora | `paola.vargas@correo.com` | `Instructor2026*` | Digital Marketing (`Marketing digital`) |

### Pre-seeded Participant Accounts (Role: `Participant`)
All 30 participants from `SkillHub_Data.xlsx` are also pre-seeded at startup with password **`Skillhub2026*`** (for example: `jorge.perez@correo.com`, `luz.ospina@correo.com`, `carlos.rincon@correo.com`, `valentina.gomez@correo.com`). Anyone can also register a new participant account via `POST /api/v1/auth/register` or `/Account/Register`.

---

## 6. API Endpoints Summary

| Method | Route | Access | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/v1/auth/register` | Public | Registers a new participant |
| `POST` | `/api/v1/auth/login` | Public | Authenticates a user and returns a JWT token |
| `GET` | `/api/v1/workshops` | Public | Lists upcoming workshops with available spots |
| `GET` | `/api/v1/workshops/{id}` | Public | Gets workshop details with available spots |
| `POST` | `/api/v1/workshops` | Employee | Creates a new workshop |
| `GET` | `/api/v1/workshops/{id}/enrollments` | Employee | Lists enrollments for a specific workshop |
| `GET` | `/api/v1/workshops/{id}/enrollments/export` | Employee | Exports workshop enrollments to an Excel (`.xlsx`) file |
| `POST` | `/api/v1/enrollments` | Participant | Enrolls the authenticated participant in a workshop |
| `GET` | `/api/v1/enrollments/mine` | Participant | Lists the authenticated participant's enrollments |
| `PATCH` | `/api/v1/enrollments/{id}/cancel` | Owner or Employee | Cancels an active enrollment |
| `POST` | `/api/v1/enrollments/import` | Employee | Bulk imports enrollments from an Excel file (`Enrollments` sheet) |
| `GET` | `/api/v1/enrollments/{id}/certificate` | Owner or Employee | Downloads the enrollment certificate in PDF format |
| `GET` | `/api/v1/reports/top-workshops` | Employee | Lists workshops ordered by active enrollments descending |

---

## 7. Author

- **Full Name:** Juan José Maldonado Navarro
- **Clan:** Cohorte 5
- **Email:** `juanjosemn160@gmail.com`
- **ID Document:** `1043663187`
